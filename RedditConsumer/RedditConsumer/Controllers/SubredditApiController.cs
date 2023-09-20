using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditConsumer.Models;
using RedditConsumer.Models.Reddit;
using RedditConsumer.Repositories;

namespace RedditConsumer.Controllers
{
    public class SubredditApiController : RedditApiController, ISubredditApiController
    {
        readonly IPostRepository postRepository;
        readonly IUserRepository userRepository;

        ///// <summary>
        ///// Keeps the EPOC time for beginning of the processing
        ///// </summary>
        double beginning;

        public SubredditApiController(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;

            beginning = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public async Task FetchData(string subreddit)
        {
            string after = null;

            do
            {

                WaitOnRateLimit();

                string token = await GetAccesstoken();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseOAuthAddress);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                    // Fetch the latest posts from the subreddit
                    var response = await client.GetAsync(getSubredditUrl(subreddit, after));
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        UpdateRateLimit(response.Headers);
                        var redditResponse = JsonConvert.DeserializeObject<Listing<SubredditPost>>(responseContent);
                        after = redditResponse.Data.After;

                        // Process each post
                        foreach (var child in redditResponse.Data.Children)
                        {
                            var post = child.Data;

                            if (post.Created >= beginning)
                            {
                                //Add the new user and post
                                User user = userRepository.Add(new User(post.Author));
                                postRepository.Add(new Post(post.Id, user, post.Title, post.Score));
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error fetching the data:");
                        Console.WriteLine(responseContent);
                    }
                }

            } while (after != null);
        }


        private string getSubredditUrl(string subreddit, string after)
        {
            string url = $"/r/{subreddit}/new.json?count=100";
            if (after != null)
            {
                url += $"&after={after}";
            }

            return url;
        }
    }
}

