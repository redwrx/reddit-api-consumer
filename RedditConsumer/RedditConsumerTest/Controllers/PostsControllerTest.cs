using Moq;
using RedditConsumer.Controllers;
using RedditConsumer.Models;
using RedditConsumer.Repositories;

namespace RedditConsumerTest.Controllers
{
    [TestClass]
    public class PostsControllerTests
    {
        [TestMethod]
        public void GetTopPostByVote_ShouldReturnTopPost()
        {
            // Arrange
            var subreddit = "testSubreddit";
            var expectedTopPost = new Post("post1", new User("user1"), "title", 10, subreddit);

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetTopPostByVote(subreddit)).Returns(expectedTopPost);

            var userRepositoryMock = new Mock<IUserRepository>();

            var controller = new PostsController(postRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var topPost = controller.GetTopPostByVote(subreddit);

            // Assert
            Assert.AreEqual(expectedTopPost.GetId(), topPost.GetId(), "Top post ID should match.");
        }

        [TestMethod]
        public void GetMostActiveUser_ShouldReturnMostActiveUser()
        {
            // Arrange
            var subreddit = "testSubreddit";
            var expectedUserId = "user123";
            var expectedUsername = "testUser";
            var expectedPostCount = 5;

            var postRepositoryMock = new Mock<IPostRepository>();
            postRepositoryMock.Setup(repo => repo.GetMostActiveUserWithCount(subreddit)).Returns(
                Tuple.Create(expectedUserId, expectedPostCount));

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetById(expectedUserId)).Returns(new User(expectedUsername));

            var controller = new PostsController(postRepositoryMock.Object, userRepositoryMock.Object);

            // Act
            var result = controller.GetMostActiveUser(subreddit);

            // Assert
            Assert.AreEqual(expectedUsername, result.Username, "Username should match.");
            Assert.AreEqual(expectedPostCount, result.PostCount, "Post count should match.");
        }
    }

}

