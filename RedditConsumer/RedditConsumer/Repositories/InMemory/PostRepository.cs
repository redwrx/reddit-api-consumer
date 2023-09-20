using System;
using System.Linq;
using RedditConsumer.Models;

namespace RedditConsumer.Repositories.InMemory
{
	public class PostRepository: IPostRepository
	{
        private static readonly List<Post> posts = new();

		public PostRepository()
		{
        }

        public void Add(Post post)
        {
            Post existing = posts.FirstOrDefault(p => p == post);
            if(existing != null)
            {
                existing.SetScore(post.GetScore());
            }
            else
            {
                posts.Add(post);
            }
        }

        public bool Exist(string id)
        {
            return posts.Where(p => p.GetId() == id).Any();
        }

        public Tuple<string, int> GetMostActiveUserWithCount()
        {
            var userPostCount = posts.GroupBy(p => p.GetUser().GetId()).Select(group => new
            {
                UserId = group.Key,
                Count = group.Count()
            }).OrderByDescending(g => g.Count).FirstOrDefault();

            String userId = null;
            int count = 0;
            if(userPostCount != null)
            {
                userId = userPostCount.UserId;
                count = userPostCount.Count;
            }

            return new Tuple<string, int>(userId, count);

        }

        public Post GetTopPostByVote()
        {
            return posts.OrderByDescending(post => post.GetScore()).FirstOrDefault();
        }
    }
}

