using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;

namespace UserStorageSystem.Services
{
    public class SlaveUserService : MarshalByRefObject, IService
    {
        public int Status => 2;
        private static readonly TraceSource ts = new TraceSource("CustomSource");
        private Dictionary<int, User> _tempData = new Dictionary<int, User>();
        private ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();
        private string ServiceIp { get; set; }
        private int ServicePort { get; set; }
        private TcpListener _tcpListener;

        public SlaveUserService CreateSlaveServiceInNewDomain(string domainName, KeyValuePair<int, string> netDefaults)
        {
            var result = (SlaveUserService)AppDomain.CreateDomain(domainName)
                .CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(SlaveUserService).FullName);
            result.ServiceIp = netDefaults.Value;
            result.ServicePort = netDefaults.Key;
            result.StartSlave();
            return result;
        }

        public SlaveUserService CreateSlaveServiceInNewDomain(string domainName, AppDomainSetup domainSetup, KeyValuePair<int, string> netDefaults)
        {
            AppDomain app = AppDomain.CreateDomain(domainName, null, domainSetup);
            var result = (SlaveUserService)app
                .CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(SlaveUserService).FullName);
            result.ServiceIp = netDefaults.Value;
            result.ServicePort = netDefaults.Key;
            result.StartSlave();
            return result;
        }

        public void StartSlave()
        {
            _tcpListener = new TcpListener(IPAddress.Parse(ServiceIp), ServicePort);
            SocketListener();
        }


        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            rwls.EnterReadLock();
            try
            {
                ts.TraceInformation($"SearchForUser request in SlaveService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                return _tempData.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria[] searchCriterias)
        {
            List<int> result = searchCriterias[0].Search(_tempData).ToList();
            for (int i = 1; i < searchCriterias.Length; i++)
            {
                result = result.Intersect(searchCriterias[i].Search(_tempData)).ToList();
            }
            return result;
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            rwls.EnterReadLock();
            try
            {
                ts.TraceInformation($"SearchForUser request in SlaveService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                return searchCriteria.Search(_tempData);
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        public void SocketListener()
        {
            ThreadPool.QueueUserWorkItem(async (x) =>
            {
                _tcpListener.Start();
                while (true)
                {
                    TcpClient tcpClient = null;
                    try
                    {
                        tcpClient = await _tcpListener.AcceptTcpClientAsync();
                        NetworkStream ns = tcpClient.GetStream();
                        var msg = await ReadMessage(ns);
                        if (msg.MethodInfo == "UserAdded")
                        {
                            UserAdded(msg.User, msg.Id);
                        }
                        if (msg.MethodInfo == "UserDeleted")
                        {
                            UserDeleted(msg.Id);
                        }
                        if (msg.MethodInfo == "UsersUploaded")
                        {
                            UpdateData(msg.UsersContainer);
                        }
                    }
                    finally
                    {
                        tcpClient?.Close();
                    }
                }
            });
        }

        private void UpdateData(Dictionary<int, User> tempDictionary)
        {
            rwls.EnterWriteLock();
            try
            {
                ts.TraceInformation($"UpdateData request in SlaveService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName} ");
                _tempData = tempDictionary.ToDictionary(x => x.Key, x => x.Value);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        private void UserAdded(User user, int id)
        {
            rwls.EnterWriteLock();
            try
            {
                Console.WriteLine($"Added user in{AppDomain.CurrentDomain.FriendlyName}");
                _tempData.Add(id, user);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        private void UserDeleted(int id)
        {
            rwls.EnterWriteLock();
            try
            {
                _tempData.Remove(id);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        private Task<Message> ReadMessage(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return Task.FromResult(formatter.Deserialize(stream) as Message);
        }

        public int Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpLoad()
        {
            throw new NotImplementedException();
        }

        
    }
}
