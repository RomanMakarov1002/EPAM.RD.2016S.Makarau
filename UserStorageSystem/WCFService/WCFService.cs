using System.Collections.Generic;
using UserStorageSystem;

namespace WCFService
{
    public class WcfService : IWcfService
    {
        private readonly Client _client;

        public WcfService(Client client)
        {
            _client = client;
        }

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
            return _client.Proxy.SearchForUser(searchCriterias);
        }
    }
}
