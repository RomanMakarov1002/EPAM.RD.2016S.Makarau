using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using WcfServiceLibrary;

namespace WcfService
{
    /// <summary>
    /// Wcf service provider
    /// </summary>
    public class ServiceProvider : IInstanceProvider, IContractBehavior
    {
        private UserStorageSystem.Client _client;

        public ServiceProvider(UserStorageSystem.Client client)
        {
            _client = client;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new SuperWcfService(_client);
        }

        public object GetInstance(InstanceContext instanceContext, Message message) => (GetInstance(instanceContext));


        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            disposable?.Dispose();
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        #region Not Implemented

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}