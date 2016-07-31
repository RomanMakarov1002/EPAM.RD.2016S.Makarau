using System.Configuration;


namespace UserStorageSystem
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

    public class Services : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string FilePath
        {
            get { return this["path"] as string; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return this["type"] as string; }
        }

        [ConfigurationProperty("repository")]
        public string Repository
        {
            get { return this["repository"] as string; }
        }

        [ConfigurationProperty("iterator")]
        public string Iterator
        {
            get { return this["iterator"] as string; }
        }

        [ConfigurationProperty("domainName")]
        public string DomainName
        {
            get { return this["domainName"] as string; }
        }


    }
    [ConfigurationCollection(typeof(Services))]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public Services this[int index]
        {
            get { return BaseGet(index) as Services; }
        }

        public new Services this[string index]
        {
            get { return  BaseGet(index) as Services; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Services();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Services) element).DomainName;
        }
    }


   
}
