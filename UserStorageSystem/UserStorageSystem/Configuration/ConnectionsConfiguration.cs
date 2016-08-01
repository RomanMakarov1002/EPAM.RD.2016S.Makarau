using System.Configuration;
using UserStorageSystem.Configuration;

namespace UserStorageSystem.Configuration
{
    public class ConnectionsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("ConnectionDefaults")]
        [ConfigurationCollection(typeof (Connections), AddItemName = "Connection")]
        public ConnectionsCollection Connections
        {
            get { return (ConnectionsCollection) this["ConnectionDefaults"]; }
        }

        public static ConnectionsConfiguration GetConfiguration()
        {
            return (ConnectionsConfiguration) ConfigurationManager.GetSection("Conns");
        }
    }
}
