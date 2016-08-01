using System;
using System.Collections.Generic;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;

namespace UserStorageSystem.Repository
{
    public interface IRepository : IEnumerable<User>
    {
        int Add(User user);
        IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria);
        IEnumerable<int> SearchForUser(Predicate<User>[] criteria);
        void Delete(int id);
        void SaveToXml(int id);
        int UpLoadFromXml();
        Dictionary<int, User> ReturnData();
    }
}
