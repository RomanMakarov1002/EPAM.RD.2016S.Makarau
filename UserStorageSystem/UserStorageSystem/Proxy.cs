﻿using System;
using System.Collections.Generic;

namespace UserStorageSystem
{
    [Serializable]
    public class Proxy :IService
    {
        public int Status => 0;
        private readonly int _servicesCount;
        private readonly List<IService> _services;

        public Proxy (int n, List<IService> services)
        {
            _servicesCount = n;
            _services = services;
        }

        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            return _services[new Random().Next(_servicesCount-1) + 1].SearchForUser(searchCriteria);
        }

        public IEnumerable<int> SearchForUser(Predicate<User>[] searchCriteria)
        {
            return _services[new Random().Next(_servicesCount-1) +1].SearchForUser(searchCriteria);
        }

        public int Add(User user)
        {
            return _services[0].Add(user);
        }

        public void Delete(int id)
        {
            _services[0].Delete(id);
        }

        public void Save()
        {
            _services[0].Save();
        }

        public void UpLoad()
        {
             _services[0].UpLoad();
        }
    }
}
