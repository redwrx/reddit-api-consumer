
using RedditConsumer.Controllers;
using RedditConsumer.Repositories;
using RedditConsumer.Repositories.InMemory;

class Program
{

    static IUserRepository userRepository = new UserRepository();

    static IPostRepository postRepository = new PostRepository();

    static PostsController postsController = new PostsController(postRepository, userRepository);

    static SubredditApiController redditApiController = new SubredditApiController(postRepository, userRepository);

    static async Task Main(string[] args)
    {

        // Step 2: Make API Requests with the Access Token
        await MakeApiRequest();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }


    static async Task MakeApiRequest()
    {

        while (true)
        {


            await redditApiController.FetchData("politics");

            // Find and display the post with the most upvotes
            //var topPost = posts.OrderByDescending(p => p.Score).FirstOrDefault();
            var topPost = postRepository.GetTopPostByVote();
            Console.WriteLine($"Top post: {topPost.GetTitle()} (Upvotes: {topPost.GetScore()})");

            // Find and display the user with the most posts
            var topUserCount = postsController.GetMostActiveUser();
            Console.WriteLine($"Top user: {topUserCount.Username} (Posts: {topUserCount.PostCount})");

            // Sleep for a while before fetching new data (e.g., every 5 minutes)
            await Task.Delay(TimeSpan.FromSeconds(10));

        }
    }
}
