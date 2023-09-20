namespace RedditConsumer.Controllers
{
    public interface ISubredditApiController
    {
        /// <summary>
        /// Fetches the data from reddit api for a subreddit and stores them in the datastore
        /// It calls the api to get pages of data for subreddits created after "beginning" datatime epoc value
        /// Blocks the thread when the rate limit reaches until the end of the reset period based on Reddit http headers
        ///
        /// </summary>
        /// <param name="subreddit"></param>
        /// <param name="beginning">EPOC time value</param>
        /// <returns></returns>
        Task FetchData(string subreddit, double beginning);
    }
}