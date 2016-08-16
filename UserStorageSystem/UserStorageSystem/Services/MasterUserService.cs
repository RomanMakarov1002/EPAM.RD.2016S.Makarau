using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UserStorageSystem.Entities;
using UserStorageSystem.Repository;
using UserStorageSystem.SearchCriterias;

namespace UserStorageSystem.Services
{
    public delegate void ModifyData(Message msg);

    /// <summary>
    /// MasterUserService class that provides Add/Delete/Search/Upload/Save methods
    /// </summary>
    public class MasterUserService : MarshalByRefObject, IService
    {
        public IRepository StorageType;
        public int Status => 1;
        public event ModifyData ModifyData = delegate { };
        private readonly TraceSource ts = new TraceSource("CustomSource");
        private int _id;
        private List<KeyValuePair<int, string>> HostsAndPorts { get; set; }
        private ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();

        public MasterUserService(IRepository storageType)
        {
            if (storageType == null)
                throw new ArgumentNullException();
            StorageType = storageType;
        }

        public MasterUserService()
        {
        }

        /// <summary>
        /// Creates slave service in new domain
        /// </summary>
        /// <param name="domainName">domain name</param>
        /// <param name="repositoryType">type of repository</param>
        /// <param name="defaultNetSettings">default ports and ip adresses</param>
        public IService CreateInstanceInNewDomain(string domainName, IRepository repositoryType, List<KeyValuePair<int, string>> defaultNetSettings)
        {
            if (domainName == null || repositoryType == null)
            {
                ts.TraceInformation($"Argument null exception during master service creation in {AppDomain.CurrentDomain.FriendlyName}");
                throw new ArgumentNullException();
            }
            var result = (MasterUserService)
                AppDomain.CreateDomain(domainName).CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                    typeof(MasterUserService).FullName);
            result.StorageType = repositoryType;
            result.HostsAndPorts = defaultNetSettings;
            return result;
        }

        /// <summary>
        /// Creates slave service in new domain
        /// </summary>
        /// <param name="domainName">domain name</param>
        /// <param name="domainSetup">domain setup</param>
        /// <param name="repositoryType">type of repository</param>
        /// <param name="defaultNetSettings">default ports and ip adresses</param>
        public IService CreateInstanceInNewDomain(string domainName, AppDomainSetup domainSetup, IRepository repositoryType, List<KeyValuePair<int, string>> defaultNetSettings)
        {
            if (domainName == null || repositoryType == null || domainSetup == null)
            {
                ts.TraceInformation($"Argument null exception during master service creation in {AppDomain.CurrentDomain.FriendlyName}");
                throw new ArgumentNullException();
            }
            AppDomain app = AppDomain.CreateDomain(domainName, null, domainSetup);
            var result = (MasterUserService)
                app.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                    typeof(MasterUserService).FullName);
            result.StorageType = repositoryType;
            result.HostsAndPorts = defaultNetSettings;
            return result;
        }

        /// <summary>
        /// Add user to repository
        /// </summary>
        /// <param name="user"></param>
        public int Add(User user)
        {
            rwls.EnterWriteLock();
            try
            {
                ts.TraceInformation($"Add user request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                if (ReferenceEquals(null, user))
                {
                    ts.TraceInformation($"Argument null exception in add user request at master in {AppDomain.CurrentDomain.FriendlyName}");
                    throw new ArgumentNullException();
                }
                if (!user.IsValid())
                {
                    ts.TraceInformation($"Validation exception in add user request at master in {AppDomain.CurrentDomain.FriendlyName}");
                    throw new ArgumentException("Validation error : Incorrect user entity");
                }
                _id = StorageType.Add(user);
                OnModify(new Message("UserAdded", _id, user, null));
                return _id;
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        /// <summary>
        /// Delete user from repository
        /// </summary>
        /// <param name="id">User's id</param>
        public void Delete(int id)
        {
            rwls.EnterWriteLock();
            try
            {
                ts.TraceInformation($"Delete user request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                StorageType.Delete(id);
                OnModify(new Message("UserDeleted", id, null, null));
            }
            finally
            {
               rwls.ExitWriteLock();
            }
        }

        /// <summary>
        /// Search users by search criteria
        /// </summary>
        /// <param name="searchCriteria">interface search criteria</param>
        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                ts.TraceInformation($"Argument null exception in search request at master in {AppDomain.CurrentDomain.FriendlyName}");
                throw new ArgumentNullException();
            }
            rwls.EnterReadLock();
            try
            {
                ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                return StorageType.SearchForUser(searchCriteria);
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        /// <summary>
        /// Search users by predicates
        /// </summary>
        /// <param name="criteria">array of predicate search criterias</param>
        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            if (criteria == null)
            {
                ts.TraceInformation($"Argument null exception in search request at master in {AppDomain.CurrentDomain.FriendlyName}");
                throw new ArgumentNullException();
            }
            rwls.EnterReadLock();
            try
            {
                ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                return StorageType.SearchForUser(criteria);
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        /// <summary>
        /// Search users by search criterias
        /// </summary>
        /// <param name="searchCriterias">array of interface search criterias</param>
        public IEnumerable<int> SearchForUser(ISearchCriteria[] searchCriterias)
        {
            if (searchCriterias == null)
            {
                ts.TraceInformation($"Argument null exception in search request at master in {AppDomain.CurrentDomain.FriendlyName}");
                throw new ArgumentNullException();
            }
            rwls.EnterReadLock();
            try
            {
                List<int> result = StorageType.SearchForUser(searchCriterias[0]).ToList();
                for (int i = 1; i < searchCriterias.Length; i++)
                {
                    result = result.Intersect(StorageType.SearchForUser(searchCriterias[i])).ToList();
                }
                return result;
            }
            finally
            {
                rwls.ExitReadLock();
            }
        }

        /// <summary>
        /// Save repository to remote file
        /// </summary>
        public void Save()
        {
            rwls.EnterWriteLock();
            try
            {
                ts.TraceInformation(
                    $"Save request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                StorageType.SaveToXml(_id);
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        /// <summary>
        /// Upload users from remote file
        /// </summary>
        public void UpLoad()
        {
            rwls.EnterWriteLock();
            try
            {
                ts.TraceInformation($"Upload request in MasterService at {DateTime.Now} in {AppDomain.CurrentDomain.FriendlyName}");
                int id = StorageType.UpLoadFromXml();
                OnModify(new Message("UsersUploaded", 0, null, StorageType.ReturnData()));
            }
            finally
            {
                rwls.ExitWriteLock();
            }
        }

        /// <summary>
        /// Sending messages to slaves about changes in repository
        /// </summary>
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
