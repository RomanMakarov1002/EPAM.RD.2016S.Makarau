using System.Configuration;


namespace UserStorageSystem
{
    public class FilePathsConfigurator : ConfigurationSection
    {
        [ConfigurationProperty("XMLFilePath")]
        [ConfigurationCollection(typeof (FilePath), AddItemName = "FilePath")]
        public FilePathCollection FilePaths
        {
            get { return (FilePathCollection) this["XMLFilePath"] ;}
        }

        public static FilePathsConfigurator GetConfiguration()
        {
            return (FilePathsConfigurator) ConfigurationManager.GetSection("FilePaths");
        }
    }

    public class FilePath : ConfigurationElement
    {
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return this["path"] as string; }
        }
    }

    [ConfigurationCollection(typeof (FilePath))]
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
            return ((FilePath) element).Path;
        }
    }
}
