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
        public void UserStorageSystem_NullArgument_ArgumentNullException()
        {
            var userStorage = new UserStorage();
            userStorage.Storage.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserStorageSystem_IncorrectUser_ValidationException()
        {
            var userStorage = new UserStorage();
            var user = new User("Alex", " ", DateTime.Now, 123, Gender.Male, null);
            userStorage.Storage.Add(user);
        }

        [TestMethod]
        public void UserStorageSystemAddUser_CorrectUser_ReturnId()
        {
            var userStorage = new UserStorage();
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            Assert.AreEqual(typeof(int), userStorage.Storage.Add(user).GetType());
        }

        [TestMethod]
        public void UserStorageSystemDeleteUser_CorrectUser()
        {
            var userStorage = new UserStorage();
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            userStorage.Storage.Add(user);
            userStorage.Storage.Add(user1);
            userStorage.Storage.Delete(1);
        }

        [TestMethod]
        public void UserStorageSystemSearchUser_ISearchCriteria_ReturnUserIds()
        {
            var userStorage = new UserStorage();
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            userStorage.Storage.Add(user);
            userStorage.Storage.Add(user1);
            Assert.AreEqual(1, userStorage.Storage.SearchForUser(new SearchByFirstName("Alex")).First());
        }

        [TestMethod]
        public void UserStorageSystemSearchUser_Predicate_ReturnUserIds()
        {
            var userStorage = new UserStorage();
            var user = new User("Alex", "Alex", DateTime.Now, 123, Gender.Male, null);
            var user1 = new User("Ben", "Larrson", DateTime.Now, 357, Gender.Male, null);
            userStorage.Storage.Add(user);
            userStorage.Storage.Add(user1);
            var results =
                userStorage.Storage.SearchForUser(new Predicate<User>[]
                { delegate(User x) { return x.Gender == Gender.Male; }});
            IEnumerable<int> expectedResult = new  List<int>(){1, 2};
            Assert.AreEqual(true,  results.SequenceEqual(expectedResult));
        }
    }
}
