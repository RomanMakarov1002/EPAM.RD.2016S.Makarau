using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;
using System.IO;

namespace Tests
{
    [TestClass]
    public class UserStorageSystem_Tests
    {
        AppDomainSetup appDomainSetup = new AppDomainSetup
        {
            ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
            PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyDomain")
        };


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Client_NullArgument_ArgumentNullException()
        {
           var client1 = new Client(null);
        }

        [TestMethod]
        public void Client_SkipParam_CreateWithDefaultValue()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()),null, appDomainSetup);
            Assert.IsTrue(client.Services.Count > 1);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Service_IncorrectUser_ValidationException()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", " ", DateTime.Now, 123, Gender.Male, null);
            client.Proxy.Add(user);
        }

        [TestMethod]
        public void MasterServiceAddUser_CorrectUser_ReturnId()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Proxy.Add(user).GetType());
        }

        [TestMethod]
        public void MasterServiceDeleteUser_CorrectUser()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            client.Proxy.Delete(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceAddUser_CorrectUser_ReturnId()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Services[1].Add(user).GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceDeleteUser_CorrectUser()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            client.Services[1].Delete(1);
        }

        [TestMethod]
        public void MasterServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Assert.AreEqual(1, client.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void SlaveServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Assert.AreEqual(1, client.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void MasterServiceSearchUser_Predicate_ReturnUserIds()
        {
            var client = new Client(new MemoryRepository(new CustomIterator()), null, appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            var results =
                 client.Proxy.SearchForUser(new Predicate<User>[]
                { delegate(User x) { return x.Gender == Gender.Male; }});
            IEnumerable<int> expectedResult = new List<int>() { 1, 2 };
            Assert.AreEqual(true, results.SequenceEqual(expectedResult));
        }   
    }
}
