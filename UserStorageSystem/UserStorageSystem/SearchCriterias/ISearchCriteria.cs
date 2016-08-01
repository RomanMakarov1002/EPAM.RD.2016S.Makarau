using System.Collections.Generic;
using UserStorageSystem.Entities;

namespace UserStorageSystem.SearchCriterias
{
    public interface ISearchCriteria
    {
        List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable);
    }
}
