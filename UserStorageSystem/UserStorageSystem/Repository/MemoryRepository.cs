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
    [Serializable]
    public class MemoryRepository : IRepository
    {
        private Dictionary<int, User> _users = new Dictionary<int, User>();
        private readonly IEnumerator<int> _enumerator = new CustomIterator();
        private readonly string _xmlPath;

        public MemoryRepository(IEnumerator<int> enumerator, string path)
        {
            if (enumerator != null)
                _enumerator = enumerator;
            if (path != null)
                _xmlPath = path;
        }

        public MemoryRepository() { }

        public int Add(User user)
        {
            _enumerator.MoveNext();
            _users.Add(_enumerator.Current, user);
            return _enumerator.Current;
        }

        public void Delete(int id)
        {
            _users.Remove(id);
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            return searchCriteria.Search(_users);
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] criteria)
        {
            return _users.Where(x => criteria.All(e => e.Invoke(x.Value))).Select(x => x.Key);
        }

        public void SaveToXml(int id)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServiceState));

            TextWriter tw = new StreamWriter(_xmlPath);
            xmlSerializer.Serialize(tw, new ServiceState() { GeneratedId = id, Users = _users.Values.ToList() });
            tw.Close();
        }

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
