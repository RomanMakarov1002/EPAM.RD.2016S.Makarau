using System;
using System.Linq;
using System.Threading;
using UserStorageSystem;


namespace ConsoleApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Visa visa  = new Visa("Monaco", DateTime.Now, DateTime.Now);
            User user = new User("Aaro" , "Ramsey", DateTime.Now, 123, Gender.Female, new Visa[] { visa });
            User user1 = new User("Aaron", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });
            User user2 = new User("Jack", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });
            User user3 = new User("Matheu", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });
            
            //Client client = new Client(new MemoryStorage(), new CustomIterator());
            //client.Proxy.AddUser(user);
            //client.Proxy.AddUser(user1);
            //client.Proxy.AddUser(user2);
            //client.Proxy.AddUser(user3);

            //client.Services[0].Save();

            //client.Services[0].UpLoad();

            Client client2 = new Client(new MemoryRepository(), new CustomIterator());
            client2.Proxy.UpLoad();
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());



            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);

       
            
           // AppDomain.Unload(Client.domain);
            Console.WriteLine("Domain unloaded");
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            // Console.WriteLine(AppDomain.);
            Console.WriteLine("user added with id");
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Console.WriteLine(client2.Proxy.AddUser(new User ("Alex",  "Alexov",  DateTime.Now, 12414141, Gender.Female,  null )));

            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Thread.Sleep(100);
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Thread.Sleep(100);
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Thread.Sleep(100);
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Thread.Sleep(100);
            Console.WriteLine("Alex Search");
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());

            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Aaro")).Count());
            client2.Proxy.DeleteUser(1);
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Aaro")).Count());

            Console.WriteLine("done");

            Console.ReadKey();
        }
    }
}
