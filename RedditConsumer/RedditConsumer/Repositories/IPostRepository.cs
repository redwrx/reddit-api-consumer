using System;
using RedditConsumer.Models;

namespace RedditConsumer.Repositories
{
	public interface IPostRepository
	{
        bool Exist(string id);
        Tuple<string, int> GetMostActiveUserWithCount(string subreddit);
        Post GetTopPostByVote(string subrreddit);
        void Add(Post post);
    }
}

