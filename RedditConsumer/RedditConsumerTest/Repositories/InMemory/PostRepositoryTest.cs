using AutoFixture;
using RedditConsumer.Models;
using RedditConsumer.Repositories.InMemory;
namespace RedditConsumerTest.Repositories.InMemory
{
    [TestClass]
    public class PostRepositoryTests
    {
        [TestMethod]
        public void Add_ShouldAddPostToList()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var repository = new PostRepository();
            var post = fixture.Create<Post>();

            // Act
            repository.Add(post);

            // Assert
            Assert.IsTrue(repository.Exist(post.GetId()), "Post should exist in the repository.");
        }

        [TestMethod]
        public void Add_ShouldUpdateScoreIfPostExists()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var repository = new PostRepository();
            var post = fixture.Create<Post>();
            repository.Add(post); // Add the initial post
            var updatedPost = new Post(post.GetId(), post.GetUser(), post.GetTitle(), post.GetScore() + 1, post.GetSubreddit());

            // Act
            repository.Add(updatedPost);

            // Assert
            var retrievedPost = repository.GetTopPostByVote(updatedPost.GetSubreddit());
            Assert.AreEqual(updatedPost.GetScore(), retrievedPost.GetScore(), "Post score should be updated.");
        }

        [TestMethod]
        public void Exist_ShouldReturnTrueIfPostExists()
        {
            // Arrange
            var fixture = new Fixture();
            var repository = new PostRepository();
            var post = fixture.Create<Post>();
            repository.Add(post);

            // Act
            var exists = repository.Exist(post.GetId());

            // Assert
            Assert.IsTrue(exists, "Post should exist in the repository.");
        }

        [TestMethod]
        public void GetMostActiveUserWithCount_ShouldReturnMostActiveUser()
        {
            // Arrange
            var repository = new PostRepository();
            var subreddit = "testSubreddit";

            // Create posts from different users in the same subreddit
            var user1 = new User("user1");
            var user2 = new User("user2");
            var user3 = new User("user3");


            repository.Add(new Post("post1", user1, "title", 1, subreddit));
            repository.Add(new Post("post2", user2, "title", 2, subreddit));
            repository.Add(new Post("post3", user3, "title", 3, subreddit));
            repository.Add(new Post("post4", user1, "title", 4, subreddit));

            // Act
            var (mostActiveUser, postCount) = repository.GetMostActiveUserWithCount(subreddit);

            // Assert
            Assert.AreEqual(user1.GetId(), mostActiveUser, "Most active user should be 'user1'.");
            Assert.AreEqual(2, postCount, "User 'user1' should have 2 posts in the subreddit.");
        }


        [TestMethod]
        public void GetTopPostByVote_ShouldReturnPostWithHighestScore()
        {
            // Arrange
            var repository = new PostRepository();
            var subreddit = "testSubreddit";

            // Create posts with different scores in the same subreddit
            var post1 = new Post("post1", new User("user1"), "title", 10, subreddit);
            var post2 = new Post("post2", new User("user2"), "title", 20, subreddit);
            var post3 = new Post("post3", new User("user3"), "title", 15, subreddit);

            repository.Add(post1);
            repository.Add(post2);
            repository.Add(post3);

            // Act
            var topPost = repository.GetTopPostByVote(subreddit);

            // Assert
            Assert.AreEqual(post2.GetId(), topPost.GetId(), "Post with the highest score should be 'post2'.");
            Assert.AreEqual(20, topPost.GetScore(), "The highest score should be 20.");
        }


    }

}

