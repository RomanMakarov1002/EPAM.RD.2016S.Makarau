using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public interface IService
    {
        int Status { get; }    // 1 - master, 2+ - slave
        int Add(User user);
        IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria);
        IEnumerable<int> SearchForUser(Predicate<User>[] criteria);
        void Delete(int id);
        void Save();
        void UpLoad();
    }
}
