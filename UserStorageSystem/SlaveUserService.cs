using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UserStorageSystem
{
    public class SlaveUserService : IService
    {
        public int Status => 2;
        private static readonly TraceSource ts = new TraceSource("CustomSource");
        private Dictionary<int, User> _tempData; 

        public SlaveUserService (Client client ,IStorage storageType)
        {
            _tempData = storageType.ReturnData();
            client.Modify += UpdateData;
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            ts.TraceInformation($"SearchForUser request in SlaveService at {DateTime.Now}");
            return _tempData.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            ts.TraceInformation($"SearchForUser request in SlaveService at {DateTime.Now}");
            return searchCriteria.Search(_tempData.AsEnumerable());
        }

        private void UpdateData(Dictionary<int, User> tempDictionary)
        {
            _tempData = tempDictionary;
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
