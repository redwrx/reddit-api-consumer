using System;
namespace RedditConsumerTest.Repositories.InMemory
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedditConsumer.Models;
    using RedditConsumer.Repositories.InMemory;
    using System;

    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void Add_ShouldAddNewUser()
        {
            // Arrange
            var repository = new UserRepository();
            var user = new User("user123");

            // Act
            var addedUser = repository.Add(user);

            // Assert
            Assert.IsNotNull(addedUser, "Added user should not be null.");
            Assert.AreEqual("user123", addedUser.GetUsername(), "Username of the added user should match.");
        }

        [TestMethod]
        public void Add_ShouldReturnExistingUserIfUsernameExists()
        {
            // Arrange
            var repository = new UserRepository();
            var user1 = new User("user123");
            var user2 = new User("user123");

            // Act
            var addedUser1 = repository.Add(user1);
            var addedUser2 = repository.Add(user2);

            // Assert
            Assert.IsNotNull(addedUser1, "Added user should not be null.");
            Assert.IsNotNull(addedUser2, "Added user should not be null.");
            Assert.AreSame(addedUser1, addedUser2, "Adding users with the same username should return the same user object.");
        }

        [TestMethod]
        public void GetById_ShouldReturnUserById()
        {
            // Arrange
            var repository = new UserRepository();
            var user1 = new User("user123");
            var user2 = new User("user456");

            repository.Add(user1);
            repository.Add(user2);

            // Act
            var retrievedUser = repository.GetById(user1.GetId());

            // Assert
            Assert.IsNotNull(retrievedUser, "Retrieved user should not be null.");
            Assert.AreEqual(user1.GetId(), retrievedUser.GetId(), "User ID should match the retrieved user.");
        }

        [TestMethod]
        public void GetById_ShouldReturnNullIfUserNotFound()
        {
            // Arrange
            var repository = new UserRepository();

            // Act
            var retrievedUser = repository.GetById("nonexistentId");

            // Assert
            Assert.IsNull(retrievedUser, "Retrieved user should be null for a non-existent ID.");
        }
    }

}

