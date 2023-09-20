using RedditConsumer.Models;
using RedditConsumer.Models.Dto;

namespace RedditConsumer.Controllers
{
    public interface IPostsController
    {
        UserPostCount GetMostActiveUser();
        Post GetTopPostByVote();
    }
}