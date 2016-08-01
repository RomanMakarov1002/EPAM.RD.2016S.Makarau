using System;
using System.Configuration;


namespace UserStorageSystem.Configuration
{
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
}
