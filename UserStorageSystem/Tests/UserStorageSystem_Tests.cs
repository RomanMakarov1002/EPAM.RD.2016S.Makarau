using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;

namespace Tests
{
    [TestClass]
    public class UserStorageSystem_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Client_NullArgument_ArgumentNullException()
        {
           Client client = new Client(null, null);
        }

        [TestMethod]
        public void Client_SkipParam_CreateWithDefaultValue()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            //Assert.IsTrue(client.Proxy.Count > 1);
        }

        [TestMethod]
        public void Client_Count_CreateCountServices()
        {
            int count = 5;
            Client client = new Client(new MemoryRepository(), new CustomIterator(), count);
            //Assert.IsTrue(client.Services.Count == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Service_IncorrectUser_ValidationException()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", " ", DateTime.Now, 123, Gender.Male, null);
            client.Proxy.Add(user);
        }

        [TestMethod]
        public void MasterServiceAddUser_CorrectUser_ReturnId()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Proxy.Add(user).GetType());
        }

        [TestMethod]
        public void MasterServiceDeleteUser_CorrectUser()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
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
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Proxy.Add(user).GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceDeleteUser_CorrectUser()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            client.Proxy.Delete(1);
        }

        [TestMethod]
        public void MasterServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Assert.AreEqual(1, client.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void SlaveServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Assert.AreEqual(1, client.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void MasterServiceSearchUser_Predicate_ReturnUserIds()
        {
            Client client = new Client(new MemoryRepository(), new CustomIterator());
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
