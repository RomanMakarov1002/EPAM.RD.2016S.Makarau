using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem
{
    public class SlaveUserService : IService
    {
        public int Status => 2;

        private Dictionary<int, User> _tempData; 

        public SlaveUserService (Client client ,IStorage storageType)
        {
            _tempData = storageType.ReturnData();
            client.Modify += UpdateData;
        }

        public void UpdateData(Dictionary<int, User> tempDictionary)
        {
            _tempData = tempDictionary;
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            return _tempData.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            return searchCriteria.Search(_tempData.AsEnumerable());
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
