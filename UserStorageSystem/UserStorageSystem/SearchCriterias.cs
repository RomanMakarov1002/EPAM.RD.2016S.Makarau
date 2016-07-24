using System;
using System.Collections.Generic;
using System.Linq;


namespace UserStorageSystem
{
    [Serializable]
    public class SearchById : ISearchCriteria
    {
        private readonly int _searchTerm;

        public SearchById(int searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Key == _searchTerm).Select(x => x.Key).ToList();
        }
    }

    [Serializable]
    public class SearchByFirstName : ISearchCriteria
    {
        private readonly string _searchTerm;

        public SearchByFirstName(string searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Value.FirstName == _searchTerm).Select(x => x.Key).ToList();
        }
    }

    [Serializable]
    public class SearchByGender : ISearchCriteria
    {
        private readonly Gender _searchTerm;

        public SearchByGender(Gender searchTerm)
        {
            _searchTerm = searchTerm;
        }

        public List<int> Search(IEnumerable<KeyValuePair<int, User>> enumerable)
        {
            return enumerable.Where(x => x.Value.Gender == _searchTerm).Select(x => x.Key).ToList();
        }
    }
}
