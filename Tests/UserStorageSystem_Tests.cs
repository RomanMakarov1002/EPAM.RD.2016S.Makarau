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
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            Assert.IsTrue(client.Services.Count > 1);
        }

        [TestMethod]
        public void Client_Count_CreateCountServices()
        {
            int count = 5;
            Client client = new Client(new MemoryStorage(), new CustomIterator(), count);
            Assert.IsTrue(client.Services.Count == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Service_IncorrectUser_ValidationException()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", " ", DateTime.Now, 123, Gender.Male, null);
            client.Services[0].Add(user);
        }

        [TestMethod]
        public void MasterServiceAddUser_CorrectUser_ReturnId()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Services[0].Add(user).GetType());
        }

        [TestMethod]
        public void MasterServiceDeleteUser_CorrectUser()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Services[0].Add(user);
            client.Services[0].Add(user1);
            client.Services[0].Delete(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceAddUser_CorrectUser_ReturnId()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Services[1].Add(user).GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceDeleteUser_CorrectUser()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Services[0].Add(user);
            client.Services[0].Add(user1);
            client.Services[1].Delete(1);
        }

        [TestMethod]
        public void MasterServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Services[0].Add(user);
            client.Services[0].Add(user1);
            Assert.AreEqual(1, client.Services[0].SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void SlaveServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Services[0].Add(user);
            client.Services[0].Add(user1);
            Assert.AreEqual(1, client.Services[1].SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void MasterServiceSearchUser_Predicate_ReturnUserIds()
        {
            Client client = new Client(new MemoryStorage(), new CustomIterator());
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Services[0].Add(user);
            client.Services[0].Add(user1);
            var results =
                 client.Services[0].SearchForUser(new Predicate<User>[]
                { delegate(User x) { return x.Gender == Gender.Male; }});
            IEnumerable<int> expectedResult = new List<int>() { 1, 2 };
            Assert.AreEqual(true, results.SequenceEqual(expectedResult));
        }

     
    }
}
