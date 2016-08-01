using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class ServiceConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        [ConfigurationCollection(typeof(Services), AddItemName = "Service")]
        public ServicesCollection Services
        {
            get { return (ServicesCollection)this["Services"]; }
        }

        public static ServiceConfiguration GetConfiguration()
        {
            return (ServiceConfiguration)ConfigurationManager.GetSection("ServiceDefaults");
        }
    }
}
