using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem
{
    public class MasterUserService : IService
    {
        public IStorage StorageType { get; }
        private readonly Client _client;
        public int Status => 1;

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
            StorageType.Delete(id);

            _client.OnModify(StorageType.ReturnData());
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            return StorageType.SearchForUser(searchCriteria);
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            return StorageType.SearchForUser(criteria);
        }

        public void Save()
        {
            StorageType.SaveToXml(_id);
        }

        public void UpLoad()
        {
            int id =StorageType.UpLoadFromXml();
            while (_enumerator.Current != id)
            {
                _enumerator.MoveNext();
            }
            _client.OnModify(StorageType.ReturnData());
        }
    }
}
