using System.Configuration;

namespace UserStorageSystem.Configuration
{
    public class FilePathsConfigurator : ConfigurationSection
    {
        [ConfigurationProperty("XMLFilePath")]
        [ConfigurationCollection(typeof(FilePath), AddItemName = "FilePath")]
        public FilePathCollection FilePaths
        {
            get { return (FilePathCollection)this["XMLFilePath"]; }
        }

        public static FilePathsConfigurator GetConfiguration()
        {
            return (FilePathsConfigurator)ConfigurationManager.GetSection("FilePaths");
        }
    }
}
