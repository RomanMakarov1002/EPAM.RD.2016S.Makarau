﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace UserStorageSystem
{
    public class MemoryStorage : IStorage
    {
        private Dictionary<int, User> _users = new Dictionary<int, User>();   
        private readonly IEnumerator<int> _enumerator = new CustomIterator();

        public int Add(int id, User user)
        {
            _users.Add(id, user);
            return id;
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

        public void SaveToXml(int id)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServiceState));

            TextWriter tw = new StreamWriter(ConfigurationManager.AppSettings["XmlFilePath"]);
            xmlSerializer.Serialize(tw, new ServiceState() { GeneratedId = id,Users = _users.Values.ToList()});
            tw.Close();
        }

        public int UpLoadFromXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServiceState));
            FileStream file = new FileStream(ConfigurationManager.AppSettings["XmlFilePath"], FileMode.Open);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int) file.Length);
            MemoryStream ms = new MemoryStream(buffer);
            var storedResults = (ServiceState)xmlSerializer.Deserialize(ms);
            _users = new Dictionary<int, User>(storedResults.Users.Count);

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
