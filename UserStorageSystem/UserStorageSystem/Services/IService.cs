using System;
using System.Collections.Generic;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;

namespace UserStorageSystem.Services
{
    public interface IService
    {
        int Status { get; }    // 1 - master, 2+ - slave
        int Add(User user);
        IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria);
        IEnumerable<int> SearchForUser(Predicate<User>[] criteria);
        IEnumerable<int> SearchForUser(ISearchCriteria[] searchCriterias);
        void Delete(int id);
        void Save();
        void UpLoad();
    }
}
