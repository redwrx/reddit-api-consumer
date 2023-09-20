
using Microsoft.Extensions.DependencyInjection;
using RedditConsumer.Controllers;
using RedditConsumer.Repositories;
using RedditConsumer.Repositories.InMemory;

class Program
{
    static ServiceProvider serviceProvider;

    static async Task Main(string[] args)
    {
        serviceProvider = new ServiceCollection()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IPostRepository, PostRepository>()
            .AddSingleton<IPostsController, PostsController>()
            .AddSingleton<ISubredditApiController, SubredditApiController>()
            .BuildServiceProvider();

        // Step 2: Make API Requests with the Access Token
        await MakeApiRequest();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }


    static async Task MakeApiRequest()
    {
        var postsController = serviceProvider.GetService<IPostsController>();
        var redditApiController = serviceProvider.GetService<ISubredditApiController>();


        while (true)
        {


            await redditApiController.FetchData("Fantasy_Football");

            // Find and display the post with the most upvotes
            //var topPost = posts.OrderByDescending(p => p.Score).FirstOrDefault();
            var topPost = postsController.GetTopPostByVote();
            if (topPost != null)
            {
                Console.WriteLine($"Top post: {topPost.GetTitle()} (Upvotes: {topPost.GetScore()})");
            }

            // Find and display the user with the most posts
            var topUserCount = postsController.GetMostActiveUser();
            Console.WriteLine($"Top user: {topUserCount.Username} (Posts: {topUserCount.PostCount})");

            // Sleep for a while before fetching new data (e.g., every 5 minutes)
            await Task.Delay(TimeSpan.FromSeconds(1));

        }
    }
}
