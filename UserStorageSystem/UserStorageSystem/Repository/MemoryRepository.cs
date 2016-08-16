using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;

namespace UserStorageSystem.Repository
{
    /// <summary>
    /// MemoryRepository class that provides Add/Delete/Search/Upload/Save methods and store data in dictionary
    /// </summary>
    [Serializable]
    public class MemoryRepository : IRepository
    {
        private Dictionary<int, User> _users = new Dictionary<int, User>();
        private readonly IEnumerator<int> _enumerator = new CustomIterator();
        private readonly string _xmlPath;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="enumerator">enumerator that generates id for users(Fibonacci enumerator by default)</param>
        /// <param name="path">file path</param>
        public MemoryRepository(IEnumerator<int> enumerator, string path)
        {
            if (enumerator != null)
                _enumerator = enumerator;
            if (path != null)
                _xmlPath = path;
        }

        public MemoryRepository() { }

        /// <summary>
        /// Adds user to repository
        /// </summary>
        /// <param name="user"></param>
        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            _enumerator.MoveNext();
            _users.Add(_enumerator.Current, user);
            return _enumerator.Current;
        }

        /// <summary>
        /// Delete user from repository according user's id
        /// </summary>
        /// <param name="id">User's id</param>
        public void Delete(int id)
        {
            _users.Remove(id);
        }

        /// <summary>
        /// Search users from repository according searchCriteria
        /// </summary>
        /// <param name="searchCriteria"> interface search criterias</param>
        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
                throw new ArgumentNullException();
            return searchCriteria.Search(_users);
        }

        /// <summary>
        /// Search users from repository according searchCriteria
        /// </summary>
        /// <param name="criteria">array of predicate search criterias</param>
        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException();
            return _users.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
        }

        /// <summary>
        /// Save repository to remote file
        /// </summary>
        public void SaveToXml(int id)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServiceState));

            TextWriter tw = new StreamWriter(_xmlPath);
            xmlSerializer.Serialize(tw, new ServiceState() { GeneratedId = id, Users = _users.Values.ToList() });
            tw.Close();
        }

        /// <summary>
        /// Upload repository from remote file
        /// </summary>
        public int UpLoadFromXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServiceState));
            FileStream file = new FileStream(_xmlPath, FileMode.Open);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            MemoryStream ms = new MemoryStream(buffer);
            var storedResults = (ServiceState)xmlSerializer.Deserialize(ms);
            _users = new Dictionary<int, User>(storedResults.Users.Count);
            file.Close();
            _enumerator.Reset();
            foreach (var item in storedResults.Users)
            {
                _enumerator.MoveNext();
                _users.Add(_enumerator.Current, item);
            }
            return storedResults.GeneratedId;
        }

        public Dictionary<int, User> ReturnData()
        {
            return _users;
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
