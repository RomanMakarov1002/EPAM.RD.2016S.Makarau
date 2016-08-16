using System;
using System.Collections.Generic;
using UserStorageSystem.Configuration;
using UserStorageSystem.Repository;
using UserStorageSystem.Services;

namespace UserStorageSystem
{
    /// <summary>
    /// Client class for configuration client's defaults via AppConfig
    /// </summary>
    public class Client 
    {
        public List<IService> Services = new List<IService>();      //for direct calls to services by threads (last point from task)
        public Proxy Proxy;                                         // for ordinary usage
        public string XmlFilePath;
        public List<KeyValuePair<int, string>> ConnectionSettings = new List<KeyValuePair<int, string>>();

        /// <summary>
        /// default configurator constructor
        /// </summary>
        public Client()
        {
            Configurate();
            Proxy = new Proxy(Services.Count, Services);
        }

        /// <summary>
        /// Parametrized cofigurator constructor
        /// </summary>
        /// <param name="domainSetup">domain setups</param>
        public Client(AppDomainSetup domainSetup)
        {
            if (domainSetup == null)
                throw new ArgumentNullException();
            Configurate(domainSetup);
            Proxy = new Proxy(Services.Count, Services);
        }

        /// <summary>
        /// Configurates client
        /// </summary>
        /// <param name="setup">domain setups</param>
        private void Configurate(AppDomainSetup setup = null)
        {
            ConfigurateConnections();
            ConfigurateFilePaths();
            var servicesConfiguration = ServiceConfiguration.GetConfiguration();
            int i = 0;
            foreach (var service in servicesConfiguration.Services)
            {
                var newService = (Configuration.Services)service;
                if (newService.Type == "MasterUserService")
                {
                    IRepository repository = new MemoryRepository();
                    IEnumerator<int> iterator = new CustomIterator();
                    if (newService.Iterator == "CustomFibonacci")
                        iterator = new CustomIterator();
                    if (newService.Repository == "MemoryRepository")
                        repository = new MemoryRepository(iterator, XmlFilePath);
                    if (setup != null)
                    {
                        Services.Add(new MasterUserService().CreateInstanceInNewDomain(newService.DomainName, setup,
                            repository,
                            ConnectionSettings));
                    }
                    else
                    {
                        Services.Add(new MasterUserService().CreateInstanceInNewDomain(newService.DomainName, repository,
                        ConnectionSettings));
                    }
                }
                else
                {
                    if (setup != null)
                    {
                        Services.Add(new SlaveUserService().CreateSlaveServiceInNewDomain(newService.DomainName, setup, ConnectionSettings[i]));
                        i++;
                    }
                    else
                    {
                        Services.Add(new SlaveUserService().CreateSlaveServiceInNewDomain(newService.DomainName, ConnectionSettings[i]));
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Configurates network connections
        /// </summary>
        private void ConfigurateConnections()
        {
            var connectionConfiguration = ConnectionsConfiguration.GetConfiguration();
            foreach (var connection in connectionConfiguration.Connections)
            {
                var newConnection = (Connections)connection;
                ConnectionSettings.Add(new KeyValuePair<int, string>(newConnection.Port, newConnection.Ip));
            }
        }

        /// <summary>
        /// Configurates remote files
        /// </summary>
        private void ConfigurateFilePaths()
        {
            var filePaths = FilePathsConfigurator.GetConfiguration();
            XmlFilePath = filePaths.FilePaths[0].Path;
        }
    }
}

