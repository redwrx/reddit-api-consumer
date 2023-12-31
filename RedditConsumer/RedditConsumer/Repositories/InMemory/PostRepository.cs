﻿using RedditConsumer.Models;

namespace RedditConsumer.Repositories.InMemory
{
	public class PostRepository: IPostRepository
	{
        private readonly List<Post> posts = new();

		public PostRepository()
		{
        }

        public void Add(Post post)
        {
            lock (posts)
            {
                Post existing = posts.FirstOrDefault(p => p.Equals(post));
                if (existing != null)
                {
                    existing.SetScore(post.GetScore());
                }
                else
                {

                    posts.Add(post);
                }
            }
        }

        public bool Exist(string id)
        {
            lock (posts)
            {
                return posts.Where(p => p.GetId() == id).Any();
            }
        }

        public Tuple<string, int> GetMostActiveUserWithCount(string subreddit)
        {
            lock (posts)
            {
                var userPostCount = posts
                .Where(p => p.GetSubreddit() == subreddit)
                .GroupBy(p => p.GetUser().GetId()).Select(group => new
                {
                    UserId = group.Key,
                    Count = group.Count()
                }).OrderByDescending(g => g.Count).FirstOrDefault();

                String userId = null;
                int count = 0;
                if (userPostCount != null)
                {
                    userId = userPostCount.UserId;
                    count = userPostCount.Count;
                }

                return new Tuple<string, int>(userId, count);
            }

        }

        public Post GetTopPostByVote(string subreddit)
        {
            lock (posts)
            {
                return posts
                .Where(p => p.GetSubreddit() == subreddit)
                .OrderByDescending(post => post.GetScore()).FirstOrDefault();
            }
        }
    }
}

