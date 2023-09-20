
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedditConsumer.Controllers;
using RedditConsumer.Factories;
using RedditConsumer.Repositories;
using RedditConsumer.Repositories.InMemory;

class Program
{
    static ServiceProvider serviceProvider;

    ///// <summary>
    ///// Keeps the EPOC time for beginning of the processing
    ///// </summary>
    static double beginning;

    /// <summary>
    /// Subreddits for this app
    /// </summary>
    static string[] subreddits;

    static async Task Main(string[] args)
    {
        serviceProvider = new ServiceCollection()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IPostRepository, PostRepository>()
            .AddSingleton<IPostsController, PostsController>()
            .AddSingleton<ISubredditApiController, SubredditApiController>()
            .AddTransient<IHttpClientFactory, HttpClientFactory>()
            .BuildServiceProvider();

        beginning = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        subreddits = ConfigurationManager.AppSettings.Get("subreddits").Split(",");


        foreach(var subreddit in subreddits)
        {
            Task.Run(() =>
            {
                fetchSubredditData(subreddit);
            });
        }

        await showResult();

    }

    static async Task fetchSubredditData(string subreddit)
    {
        var redditApiController = serviceProvider.GetService<ISubredditApiController>();
        while (true)
        {
            try
            {
                await redditApiController.FetchData(subreddit, beginning);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unable to fetch data for {subreddit}. Unexpected exception: {e.Message}");
            }
        }
    }




    static async Task showResult()
    {
        var postsController = serviceProvider.GetService<IPostsController>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"----------------{DateTime.Now.ToShortTimeString()}----------------");
            foreach (string subreddit in subreddits)
            {
                Console.WriteLine();
                Console.WriteLine($"****** {subreddit} ******");
                Console.WriteLine();
                var topPost = postsController.GetTopPostByVote(subreddit);
                if (topPost != null)
                {
                    Console.WriteLine($"Top Post(Upvotes => {topPost.GetScore()}):");
                    Console.WriteLine(topPost.GetTitle());
                }
                else
                {
                    Console.WriteLine("No posts yet!");
                    continue;
                }

                Console.WriteLine();

                var topUserCount = postsController.GetMostActiveUser(subreddit);
                if (topUserCount.Username != null)
                {
                    Console.WriteLine($"Top User(# Of Posts => {topUserCount.PostCount}):");
                    Console.WriteLine($"{topUserCount.Username}");
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5));

        }
    }
}
