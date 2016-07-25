using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace UserStorageSystem
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
        private Thread _thread;

        public SlaveUserService CreateSlaveServiceInNewDomain(string domainName, KeyValuePair<int, string> netDefaults)
        {
            var result = (SlaveUserService) AppDomain.CreateDomain(domainName)
                .CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof (SlaveUserService).FullName);
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
            _thread = new Thread(SocketListener);
            _thread.Start();
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

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            rwls.EnterReadLock();
            try
            {
                ts.TraceInformation($"SearchForUser request in SlaveService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                return searchCriteria.Search(_tempData.AsEnumerable());
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        public void SocketListener()
        {
            _tcpListener.Start();
            TcpClient tcpClient = _tcpListener.AcceptTcpClient();
            NetworkStream ns = tcpClient.GetStream();
            var formatter = new BinaryFormatter();
            while (true)
            {
                Thread.Sleep(10);
                //
                Message msg = (Message)formatter.Deserialize(ns);
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
            _thread.Join();
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
