using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public interface IRepository : IEnumerable<User>
    {
        int Add(int id, User user);
        IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria);
        IEnumerable<int> SearchForUser(Predicate<User>[] criteria);
        void Delete(int id);
        void SaveToXml(int id);
        int UpLoadFromXml();
        Dictionary<int, User> ReturnData();
    }
}
