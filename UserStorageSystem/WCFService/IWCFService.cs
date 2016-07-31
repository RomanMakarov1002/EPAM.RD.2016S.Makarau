using System.Collections.Generic;
using System.ServiceModel;
using UserStorageSystem;

namespace WCFService
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        int Add(User user);
        [OperationContract]
        IEnumerable<int> SearchForUser(params ISearchCriteria[] searchCriterias);
        [OperationContract]
        void Delete(int id);
    }
}
