using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public interface ISearchCriteria
    {
        IEnumerable<int> Search(IEnumerable<KeyValuePair<int,User>> enumerable);
    }
}
