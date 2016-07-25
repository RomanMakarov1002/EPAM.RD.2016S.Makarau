using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace UserStorageSystem
{
    [Serializable]
    public class Client 
    { 
        public List<IService> Services { get; } = new List<IService>();     //for direct calls to services by threads (last point from task)
        public Proxy Proxy;                                                 // for ordinary usage
        public static string XmlPath { get; set; } = ConfigurationManager.AppSettings["XmlFilePath"];
        private readonly int _defaultServicesCount = 
            Convert.ToInt32(
                ((NameValueCollection) ConfigurationManager.GetSection("ClientDefaults"))["NumberOfInstances"]);

        private List<KeyValuePair<int, string>> _defaultNetSettings= new List<KeyValuePair<int, string>>();
     
        public Client(IRepository storageType,  int? n = null, AppDomainSetup domainSetup = null)
        {
            if (storageType == null )
                throw new ArgumentNullException();
            if (n == null)
                n = _defaultServicesCount;
            for (int i=0; i<n-1; i++)
            {
                int port = Convert.ToInt32(
                ((NameValueCollection)ConfigurationManager.GetSection("ClientDefaults"))["ServicePort"+i]);
                string ip =
                    ((NameValueCollection) ConfigurationManager.GetSection("ClientDefaults"))["ServiceIp"+i];
                _defaultNetSettings.Add(new KeyValuePair<int, string>(port,ip));
            }
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    if (domainSetup == null)
                        Services.Add(new MasterUserService().CreateInstanceInNewDomain("MasterServiceDomain"+i, storageType, _defaultNetSettings));
                    else
                    {
                        Services.Add(new MasterUserService().CreateInstanceInNewDomain("MasterServiceDomain" + i, domainSetup, storageType, _defaultNetSettings));
                    }
                }
                else
                {
                    if (domainSetup == null)
                        Services.Add(new SlaveUserService().CreateSlaveServiceInNewDomain("SlaveServiceDomain" + i,
                            _defaultNetSettings[i - 1]));
                    else
                    {
                        Services.Add(new SlaveUserService().CreateSlaveServiceInNewDomain("SlaveServiceDomain" + i,
                          domainSetup, _defaultNetSettings[i - 1]));
                    }
                }
            }
            ((MasterUserService)Services[0]).ConnectMaster();
            Proxy = new Proxy(n.Value, Services);
        }
    }
}
