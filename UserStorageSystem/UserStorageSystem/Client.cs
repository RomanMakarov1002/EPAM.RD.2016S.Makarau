using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace UserStorageSystem
{
    [Serializable]
    public class Client 
    { 
        private List<IService> Services { get; } = new List<IService>();
        public Proxy Proxy;
        public static string XmlPath { get; set; } = ConfigurationManager.AppSettings["XmlFilePath"];
        public static AppDomain domain;
        private readonly int _defaultServicesCount = 
            Convert.ToInt32(
                ((NameValueCollection) ConfigurationManager.GetSection("ClientDefaults"))["NumberOfInstances"]);

        private List<KeyValuePair<int, string>> _defaultNetSettings= new List<KeyValuePair<int, string>>();
     
        public Client(IRepository storageType, IEnumerator<int> enumerator, int? n = null)
        {
            if (storageType == null || enumerator == null)
                throw new ArgumentNullException();
            if (n == null)
                n = _defaultServicesCount;
            for (int i=0; i<n-1; i++)
            {
                int port = Convert.ToInt32(
                ((NameValueCollection)ConfigurationManager.GetSection("ClientDefaults"))["ServicePort"+i.ToString()]);
                string ip =
                    ((NameValueCollection) ConfigurationManager.GetSection("ClientDefaults"))["ServiceIp"+i.ToString()];
                _defaultNetSettings.Add(new KeyValuePair<int, string>(port,ip));
            }
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    MasterUserService mu = new MasterUserService();
                    Services.Add(mu.CreateInstanceInNewDomain("MasterServiceDomain"+i.ToString(), storageType, enumerator, _defaultNetSettings));
                }
                else
                {
                    Services.Add(new SlaveUserService().CreateSlaveServiceInNewDomain("SlaveServiceDomain"+i, _defaultNetSettings[i-1]));
                }
            }
            ((MasterUserService)Services[0]).ConnectMaster();
            Proxy = new Proxy(n.Value, Services);
        }
    }
}
