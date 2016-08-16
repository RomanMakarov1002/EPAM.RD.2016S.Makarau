using System;
using System.ServiceModel.Description;
using UserStorageSystem;
using WcfService;

namespace WcfStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            var baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/WcfServiceLibrary/SuperWcfService/");
            var proxy = client;
            using (var host = new CustomServiceHost(proxy, typeof(WcfServiceLibrary.SuperWcfService), baseAddress))
            {
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter =
                    {
                        PolicyVersion = PolicyVersion.Default
                    }
                };
                host.Description.Behaviors.Add(smb);
                host.Open();

                Console.WriteLine("Started at {0}", baseAddress);
                Console.WriteLine("Press any button to stop service :");
                Console.ReadKey();
                host.Close();
            }
            client.Proxy.Save();
        }
    }
}
