using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageSystem.Entities;

namespace UserStorageSystem.SearchCriterias
{
    [Serializable]
    public class SearchByFirstName : ISearchCriteria
    {
        public readonly string SearchTerm;

        public SearchByFirstName(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Value.FirstName == SearchTerm).Select(x => x.Key).ToList();
        }
    }
}
