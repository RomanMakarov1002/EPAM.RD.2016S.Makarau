using System;
using System.Collections.Generic;
using UserStorageSystem;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;
using WcfService;

namespace WcfServiceLibrary
{
    public class SuperWcfService : IWcfService
    {
        private Client _client;

        public SuperWcfService(Client client)
        {
            _client = client;
        }

        public SuperWcfService() { }

        public int Add(User user)
        {
            return _client.Proxy.Add(user);
        }

        public void Delete(int id)
        {
            _client.Proxy.Delete(id);
        }

        public IEnumerable<int> SearchForUser(params ISearchCriteria[] searchCriterias)
        {
            throw new NotImplementedException();
        }
    }
}
