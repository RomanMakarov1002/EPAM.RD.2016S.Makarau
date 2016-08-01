using System;
using System.Collections.Generic;
using System.Linq;
using UserStorageSystem.Entities;

namespace UserStorageSystem.SearchCriterias
{
    [Serializable]
    public class SearchByGender : ISearchCriteria
    {
        public readonly Gender SearchTerm;

        public SearchByGender(Gender searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Value.Gender == SearchTerm).Select(x => x.Key).ToList();
        }
    }
}
