using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserStorageSystem
{
    public class MemoryStorage : IStorage
    {
        private readonly Dictionary<int, User> _users = new Dictionary<int, User>();   
        private readonly IEnumerator<int> _enumerator;
        private int _id;

        public MemoryStorage(IEnumerator<int> enumerator)
        {
            _enumerator = enumerator;
        }

        public int Add(User user)
        {
            if (ReferenceEquals(null, user))
                throw new ArgumentNullException();
            if (!user.IsValid())
                throw new ArgumentException("Validation error : Incorrect user entity");
            _enumerator.MoveNext();
            _id = _enumerator.Current;
            _users.Add(_id, user);
            return _id;
        }

        public void Delete(int id)
        {
            _users.Remove(id);
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            return searchCriteria.Search(_users.AsEnumerable());
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            return _users.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
        } 

    }
}
