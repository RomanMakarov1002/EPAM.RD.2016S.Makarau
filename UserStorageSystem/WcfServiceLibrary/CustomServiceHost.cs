using System;
using System.ServiceModel;

namespace WcfService
{
    public class CustomServiceHost: ServiceHost
    {
        public CustomServiceHost (UserStorageSystem.Client client, Type serviceType, params Uri[] baseAddresses)
        : base(serviceType, baseAddresses)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            foreach (var cd in ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new ServiceProvider(client));
            }
        }
    }
}