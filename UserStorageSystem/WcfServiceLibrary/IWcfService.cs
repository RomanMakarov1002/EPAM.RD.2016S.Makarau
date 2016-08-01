using System.Collections.Generic;
using System.ServiceModel;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
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
