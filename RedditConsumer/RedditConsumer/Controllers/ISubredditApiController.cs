namespace RedditConsumer.Controllers
{
    public interface ISubredditApiController
    {
        Task FetchData(string subreddit);
    }
}