using RedditConsumer.Models;
using RedditConsumer.Models.Dto;

namespace RedditConsumer.Controllers
{
    public interface IPostsController
    {
        UserPostCount GetMostActiveUser(string subreddit);
        Post GetTopPostByVote(string subreddit);
    }
}