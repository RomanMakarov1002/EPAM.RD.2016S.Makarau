using System.Configuration;

namespace UserStorageSystem.Configuration
{
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
            return ((Connections)element).Port;
        }
    }
}
