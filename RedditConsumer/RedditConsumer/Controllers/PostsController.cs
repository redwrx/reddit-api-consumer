using System;
using RedditConsumer.Models;
using RedditConsumer.Models.Dto;
using RedditConsumer.Repositories;

namespace RedditConsumer.Controllers
{
    public class PostsController : IPostsController
    {
        readonly IPostRepository postRepository;
        readonly IUserRepository userRepository;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public Post GetTopPostByVote(string subreddit)
        {
            return postRepository.GetTopPostByVote(subreddit);
        }

        public UserPostCount GetMostActiveUser(string subreddit)
        {
            var tuple = postRepository.GetMostActiveUserWithCount(subreddit);
            string username = null;

            if (tuple.Item1 != null)
            {
                username = userRepository.GetById(tuple.Item1).GetUsername();
            }
            return new UserPostCount { Username = username, PostCount = tuple.Item2 };
        }
    }
}

