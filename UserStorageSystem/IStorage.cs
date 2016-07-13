using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public interface IStorage
    {
        int Add(User user);
        IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria);
        IEnumerable<int> SearchForUser(Predicate<User>[] criteria);
        void Delete(int id);
    }
}
