using System.Configuration;

namespace UserStorageSystem.Configuration
{
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
}
