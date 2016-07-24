using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public interface ISearchCriteria
    {
        List<int> Search(IEnumerable<KeyValuePair<int,User>> enumerable);
    }
}
