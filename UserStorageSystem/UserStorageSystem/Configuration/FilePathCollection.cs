using System.Configuration;

namespace UserStorageSystem.Configuration
{
    [ConfigurationCollection(typeof(FilePath))]
    public class FilePathCollection : ConfigurationElementCollection
    {
        public FilePath this[int index]
        {
            get { return BaseGet(index) as FilePath; }
        }

        public new FilePath this[string index]
        {
            get { return BaseGet(index) as FilePath; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FilePath();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilePath)element).Path;
        }
    }
}
