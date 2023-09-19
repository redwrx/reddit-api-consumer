using System;
using RedditConsumer.Models;

namespace RedditConsumer.Repositories
{
	public interface IPostRepository
	{
        Tuple<string, int> GetMostActiveUserWithCount();
        Post GetTopPostByVote();
        void Add(Post post);
    }
}

