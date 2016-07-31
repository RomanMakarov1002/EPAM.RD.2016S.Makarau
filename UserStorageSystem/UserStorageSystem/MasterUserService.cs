using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;


namespace UserStorageSystem
{
    public delegate void ModifyData(Message msg);

    public class MasterUserService :MarshalByRefObject, IService   {
        public IRepository StorageType;
        public int Status => 1;
        public event ModifyData ModifyData = delegate { };
        private readonly TraceSource ts = new TraceSource("CustomSource");
        private int _id;
        private List<KeyValuePair<int, string>> HostsAndPorts {get; set; } 

        public MasterUserService(IRepository storageType )
        {
            StorageType = storageType;
        }

        public MasterUserService()
        {
        }

        public IService CreateInstanceInNewDomain(string domainName, IRepository repositoryType, List<KeyValuePair<int, string>> defaultNetSettings)
        {
            var result = (MasterUserService)
                AppDomain.CreateDomain(domainName).CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                    typeof (MasterUserService).FullName);
            result.StorageType = repositoryType;
            result.HostsAndPorts = defaultNetSettings;
            return result;
        }

        public IService CreateInstanceInNewDomain(string domainName, AppDomainSetup domainSetup, IRepository repositoryType, List<KeyValuePair<int, string>> defaultNetSettings)
        {
            AppDomain app = AppDomain.CreateDomain(domainName, null, domainSetup);
            var result = (MasterUserService)
                app.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                    typeof(MasterUserService).FullName);
            result.StorageType = repositoryType;
            result.HostsAndPorts = defaultNetSettings;
            return result;
        }

        public int Add(User user)
        {
            object obj = new object();
            Monitor.Enter(obj);
            try
            {
                ts.TraceInformation($"Add user request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                if (ReferenceEquals(null, user))
                    throw new ArgumentNullException();
                if (!user.IsValid())
                    throw new ArgumentException("Validation error : Incorrect user entity");
                _id = StorageType.Add(user);
                OnModify(new Message("UserAdded", _id, user, null));
                return _id;
            }
            finally 
            {
                Monitor.Exit(obj);
            }
        }

        public void Delete(int id)
        {
            object obj = new object();
            Monitor.Enter(obj);
            try
            {
                ts.TraceInformation($"Delete user request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                StorageType.Delete(id);
                OnModify(new Message("UserDeleted", id, null, null));
            }
            finally 
            {
               Monitor.Exit(obj);
            }
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
            return StorageType.SearchForUser(searchCriteria);
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
            return StorageType.SearchForUser(criteria);
        }

        public void Save()
        {
            ts.TraceInformation($"Save request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
            StorageType.SaveToXml(_id);
        }

        public void UpLoad()
        {
            object obj = new object();
            Monitor.Enter(obj);
            try
            {
                ts.TraceInformation($"Upload request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                int id = StorageType.UpLoadFromXml();
                OnModify(new Message("UsersUploaded", 0, null, StorageType.ReturnData()));
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }

        private async void OnModify(Message msg)
        {
            BinaryFormatter bf = new BinaryFormatter();
            TcpClient client; 
            foreach (var item in HostsAndPorts)
            {
                client = new TcpClient();
                await client.ConnectAsync(item.Value, item.Key);

                using (var networkStream = client.GetStream())
                {
                    if (networkStream.CanWrite)
                        bf.Serialize(networkStream, msg);
                }
                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
}
