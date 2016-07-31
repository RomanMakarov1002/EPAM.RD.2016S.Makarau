using System;
using System.Collections.Generic;
using System.Linq;


namespace UserStorageSystem
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
