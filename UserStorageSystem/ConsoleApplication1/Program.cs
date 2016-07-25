using System;
using System.Linq;
using System.Threading;
using UserStorageSystem;


namespace ConsoleApplication1
{
    public class Program
    {
        static CancellationTokenSource cts = new CancellationTokenSource();  
        static CancellationToken token = cts.Token;
        
        public static void Main(string[] args)
        {

            Console.WriteLine("Press any key to run ordinary check");
            Console.ReadKey();
            OrdinaryWorkCheck();
            Console.WriteLine("Press any key to run multithreading check");

            Console.ReadKey();

            Thread thread = new Thread(new ThreadStart(CheckWithMultiThreading));
            thread.Start();
            Console.ReadKey();
            cts.Cancel();
            thread.Join();
            Console.WriteLine("Bingo!");
        }

        public static void OrdinaryWorkCheck()
        {
            Visa visa = new Visa("Monaco", DateTime.Now, DateTime.Now);
            User user = new User("Aaro", "Ramsey", DateTime.Now, 123, Gender.Female, new Visa[] { visa });
            User user1 = new User("Aaron", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });
            User user2 = new User("Jack", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });
            User user3 = new User("Matheu", "Ramsey", DateTime.Now, 123, Gender.Male, new Visa[] { visa });

            Client client2 = new Client(new MemoryRepository(new CustomIterator()));
            client2.Proxy.Add(user);
            client2.Proxy.Add(user1);
            client2.Proxy.Add(user2);
            client2.Proxy.Add(user3);

            client2.Services[0].Save();

            client2.Services[0].UpLoad();

            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Console.WriteLine("user added with id");
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Jack")).First());
            Console.WriteLine(client2.Proxy.Add(new User("Alex", "Alexov", DateTime.Now, 12414141, Gender.Female, null)));

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
            client2.Proxy.Delete(1);
            Console.WriteLine(client2.Proxy.SearchForUser(new SearchByFirstName("Aaro")).Count());

            Console.WriteLine("done");
        }


        public static void CheckWithMultiThreading()
        {
            var client3 = new Client(new MemoryRepository());
            Visa visa = new Visa("Monaco", DateTime.Now, DateTime.Now);
            User user = new User("Aaro", "Ramsey", DateTime.Now, 123, Gender.Female, new Visa[] { visa });
            Action[] masterCalls = new Action[5];
            masterCalls[0] = () =>
            {
                client3.Services[0].UpLoad();

            };
            masterCalls[1] = () =>
            {
                client3.Services[0].SearchForUser(new SearchByFirstName("Aaro"));
            };
            masterCalls[2] = () =>
            {
                client3.Services[0].Add(user);

            };
            masterCalls[3] = () =>
            {
                client3.Services[0].Delete(2);
            };
            masterCalls[4] = () =>
            {
                client3.Services[0].Delete(1);
            };

            Action[] firstSlaveCalls = new Action[5];
            firstSlaveCalls[0] = () =>
            {

                Console.WriteLine($"search of user Aaro in slave {client3.Services[1].SearchForUser(new SearchByFirstName("Aaro")).Count()}");
            };
            firstSlaveCalls[1] = () =>
            {
                Console.WriteLine($"search of user Jack in slave {client3.Services[1].SearchForUser(new SearchByFirstName("Jack")).Count()}");
            };
            firstSlaveCalls[2] = () =>
            {
                Console.WriteLine($"search of user Jack in slave { client3.Services[1].SearchForUser(new SearchByFirstName("Jack")).Count()}");
            };
            firstSlaveCalls[3] = () =>
            {
                Console.WriteLine($"search of user Matheu in slave { client3.Services[1].SearchForUser(new SearchByFirstName("Matheu")).Count()}");
            };
            firstSlaveCalls[4] = () =>
            {
                Console.WriteLine($"search of user Jack in slave { client3.Services[1].SearchForUser(new SearchByFirstName("Jack")).Count()}");
            };

            Thread thread;
            Thread thread2;
            Thread thread3;
            int i = 0;
            bool cancelation = true;
            while (cancelation)
            {
                if (i == 4)
                    i = 0;
                thread = new Thread(new ThreadStart(masterCalls[i]));
                thread.Start();
                Thread.Sleep(1000);
                thread2 = new Thread(new ThreadStart(firstSlaveCalls[i]));
                thread2.Start();
                thread3 = new Thread(new ThreadStart(firstSlaveCalls[i]));
                thread3.Start();


                i++;
                thread.Join();
                thread2.Join();
                thread3.Join();
                cancelation = !token.IsCancellationRequested;
            }
        }

    }
}
