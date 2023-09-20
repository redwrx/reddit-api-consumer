using Newtonsoft.Json;
using RedditConsumer.Models;
using RedditConsumer.Models.Reddit;
using RedditConsumer.Repositories;

namespace RedditConsumer.Controllers
{
    public class SubredditApiController : RedditApiController, ISubredditApiController
    {
        readonly IPostRepository postRepository;
        readonly IUserRepository userRepository;

        public SubredditApiController(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public async Task FetchData(string subreddit)
        {
            string token = await getAccesstoken();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseOAuthAddress);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                // Fetch the latest posts from the subreddit
                string responseJson = await client.GetStringAsync($"/r/{subreddit}/new.json");

                var redditResponse = JsonConvert.DeserializeObject<Listing<SubredditPost>>(responseJson);

                // Process each post
                foreach (var child in redditResponse.Data.Children)
                {
                    var post = child.Data;

                    User user = userRepository.Add(new User(post.Author));
                    postRepository.Add(new Post(user, post.Title, post.Score));
                }
            }
        }
    }
}

