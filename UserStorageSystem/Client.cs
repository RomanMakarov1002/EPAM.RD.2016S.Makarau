using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorageSystem
{
    public delegate void Modify(Dictionary<int, User> tempDataDictionary);
    public class Client
    {
        public List<IService> Services = new List<IService>(); 
        public event Modify Modify = delegate { };
        private readonly int _defaultServicesCount =
            Convert.ToInt32(
                ((NameValueCollection) ConfigurationManager.GetSection("ClientDefaults"))["NumberOfInstances"]);


        public void OnModify(Dictionary<int, User> tempData)
        {
            Modify(tempData);
        }

        public Client(IStorage storageType, IEnumerator<int> enumerator, int? n = null)
        {
            if (n == null)
                n = _defaultServicesCount;
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    Services.Add(new MasterUserService(this, storageType, enumerator));
                }
                else
                {
                    Services.Add(new SlaveUserService(this, storageType));
                }
            }
        }
    }
}
