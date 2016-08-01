using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class FilePath : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return this["path"] as string; }
        }
    }
}
