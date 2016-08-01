using System.Configuration;

namespace UserStorageSystem.Configuration
{
    [ConfigurationCollection(typeof(Services))]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public Services this[int index]
        {
            get { return BaseGet(index) as Services; }
        }

        public new Services this[string index]
        {
            get { return BaseGet(index) as Services; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Services();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Services)element).DomainName;
        }
    }
}
