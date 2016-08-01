using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageSystem;
using System.IO;
using UserStorageSystem.Entities;
using UserStorageSystem.SearchCriterias;
using System.Linq;
using System.Threading;

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
        public void Client_SkipParam_CreateWithDefaultValue()
        {
            var client = new Client(appDomainSetup);
            Assert.IsTrue(client.Services.Count > 1);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Service_IncorrectUser_ValidationException()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", " ", DateTime.Now, 123, Gender.Male, null);
            client.Proxy.Add(user);
        }

        [TestMethod]
        public void MasterServiceAddUser_CorrectUser_ReturnId()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Proxy.Add(user).GetType());
        }

        [TestMethod]
        public void MasterServiceDeleteUser_CorrectUser()
        {
            var client = new Client(appDomainSetup);
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
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), client.Services[1].Add(user).GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SlaveServiceDeleteUser_CorrectUser()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            client.Services[1].Delete(1);
        }

        [TestMethod]
        public void MasterServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Thread.Sleep(3000);
            Assert.AreEqual(1, client.Services[0].SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void ServiceAsyncWork_ISearchCriteria_ReturnNoId()    // no thread sleep so service can't add user to local repository in time
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Assert.AreNotEqual(1, client.Services[0].SearchForUser(new SearchByFirstName("Alex")).Count()); 
        }


        [TestMethod]
        public void SlaveServiceSearchUser_ISearchCriteria_ReturnUserIds()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            Thread.Sleep(3000);
            Assert.AreEqual(1, client.Proxy.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void MasterServiceSearchUser_ISearchCriteriaS_ReturnUserIds()
        {
            var client = new Client(appDomainSetup);
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Alex", "Larrson", DateTime.Now, 357, Gender.Male, null);
            var user2 = new User("Alex", "jackson", DateTime.Now, 377, Gender.Female, null);
            client.Proxy.Add(user);
            client.Proxy.Add(user1);
            client.Proxy.Add(user2);
            Thread.Sleep(3000);
            var results = client.Proxy.SearchForUser(new SearchByFirstName("Alex"), new SearchByGender(Gender.Male));
            IEnumerable<int> expectedResult = new List<int>() { 1, 2 };
            Assert.AreEqual(true, results.SequenceEqual(expectedResult));
        }
    }
}
