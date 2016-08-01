using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageSystem.Entities;

namespace UserStorageSystem.SearchCriterias
{
    [Serializable]
    public class SearchById : ISearchCriteria
    {
        public readonly int SearchTerm;

        public SearchById(int searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Key == SearchTerm).Select(x => x.Key).ToList();
        }
    }
}
