using System;
using System.Configuration;

namespace UserStorageSystem
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

    public class Connections : ConfigurationElement
    {
        [ConfigurationProperty("port")]
        public int Port 
        {
            get { return Convert.ToInt32(this["port"]); }
        }

        [ConfigurationProperty("ip")]
        public string Ip
        {
            get { return this["ip"] as string; }
        }
    }

    [ConfigurationCollection(typeof(Connections))]
    public class ConnectionsCollection : ConfigurationElementCollection
    {
        public Connections this[int index]
        {
            get { return BaseGet(index) as Connections; }
        }

        public new Connections this[string index]
        {
            get { return BaseGet(index) as Connections; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Connections();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Connections) element).Port;
        }
    }
}
