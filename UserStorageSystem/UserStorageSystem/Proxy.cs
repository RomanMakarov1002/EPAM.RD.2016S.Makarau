using System;
using System.Collections.Generic;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;
using UserStorageSystem.Services;

namespace UserStorageSystem
{
    /// <summary>
    /// Proxy class that manages services work
    /// Search queries call slave services
    /// Add/Delete queries call master service
    /// </summary>
    [Serializable]
    public class Proxy :IService
    {
        public int Status => 0;
        private readonly int _servicesCount;
        private readonly List<IService> _services;
        private int _current = 0;

        public Proxy (int n, List<IService> services)
        {
            if (services == null)
                throw new ArgumentNullException();
            _servicesCount = n;
            _services = services;
        }

        /// <summary>
        /// Search users by search criteria
        /// </summary>
        /// <param name="searchCriteria">interface search criteria</param>
        public IEnumerable<int> SearchForUser(ISearchCriteria searchCriteria)
        {
            _current = ++_current%_servicesCount;
            return _services[_current].SearchForUser(searchCriteria);
            
        }

        /// <summary>
        /// Search users by predicates
        /// </summary>
        /// <param name="searchCriteria">array of predicate search criterias</param>
        public IEnumerable<int> SearchForUser(Predicate<User>[] searchCriteria)
        {
            if (searchCriteria == null)
                throw new ArgumentNullException();
            _current = ++_current % _servicesCount;
            return _services[_current].SearchForUser(searchCriteria);
        }

        /// <summary>
        /// Calls master service to add user
        /// </summary>
        /// <param name="user"></param>
        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            return _services[0].Add(user);
        }

        /// <summary>
        /// Calls master service to remove user
        /// </summary>
        /// <param name="id">user id</param>
        public void Delete(int id)
        {
            _services[0].Delete(id);
        }

        /// <summary>
        /// Calls master service to save current state
        /// </summary>
        public void Save()
        {
            _services[0].Save();
        }

        /// <summary>
        /// Calls master service to load state 
        /// </summary>
        public void UpLoad()
        {
             _services[0].UpLoad();
        }

        /// <summary>
        /// Search users by search criterias (using slave services)
        /// </summary>
        /// <param name="searchCriterias">array of interface search criterias</param>
        public IEnumerable<int> SearchForUser(params ISearchCriteria[] searchCriterias)
        {
            if (searchCriterias == null)
                throw new ArgumentNullException();
            _current = ++_current % _servicesCount;
            return _services[_current].SearchForUser(searchCriterias);
        }
    }
}
