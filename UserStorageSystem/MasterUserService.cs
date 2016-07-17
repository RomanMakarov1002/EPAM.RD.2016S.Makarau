using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace UserStorageSystem
{
    public class MasterUserService : IService
    {
        public IStorage StorageType { get; }
        public int Status => 1;
        private readonly Client _client;
        private static readonly TraceSource ts = new TraceSource("CustomSource");
        private readonly IEnumerator<int> _enumerator;
        private int _id;

        public MasterUserService(Client client, IStorage storageType, IEnumerator<int> enumerator )
        {
            StorageType = storageType;
            _enumerator = enumerator;
            _client = client;
        }

        public int Add(User user)
        {
            ts.TraceInformation($"Add user request in MasterService at {DateTime.Now}");
            if (ReferenceEquals(null, user))
                throw new ArgumentNullException();
            if (!user.IsValid())
                throw new ArgumentException("Validation error : Incorrect user entity");
            _enumerator.MoveNext();
            _id = _enumerator.Current;
            StorageType.Add(_id, user);
            _client.OnModify(StorageType.ReturnData());
            return _id;
        }

        public void Delete(int id)
        {
            ts.TraceInformation($"Delete user request in MasterService at {DateTime.Now}");
            StorageType.Delete(id);
            _client.OnModify(StorageType.ReturnData());
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now}");
            return StorageType.SearchForUser(searchCriteria);
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            ts.TraceInformation($"SearchForUser request in MasterService at {DateTime.Now}");
            return StorageType.SearchForUser(criteria);
        }

        public void Save()
        {
            ts.TraceInformation($"Save request in MasterService at {DateTime.Now}");
            StorageType.SaveToXml(_id);
        }

        public void UpLoad()
        {
            ts.TraceInformation($"Upload request in MasterService at {DateTime.Now}");
            int id =StorageType.UpLoadFromXml();
            while (_enumerator.Current != id)
            {
                _enumerator.MoveNext();
            }
            _client.OnModify(StorageType.ReturnData());
        }
    }
}
