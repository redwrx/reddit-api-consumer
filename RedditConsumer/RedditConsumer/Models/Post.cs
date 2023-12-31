﻿using System;
namespace RedditConsumer.Models
{
	public class Post : IEquatable<Post>
    {
        private readonly string id;
        private readonly string title;
        private int score;
        private readonly User user;
        private readonly string subreddit;

        public Post(String id, User user, string title, int score, string subreddit)
		{
            this.id = id;
            this.title = title;
			this.score = score;
			this.user = user;
            this.subreddit = subreddit;
		}

        public string GetId()
        {
            return id;
        }

        public string GetTitle()
		{
			return title;
		}

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int score)
        {
            this.score = score;
        }

        public User GetUser()
        {
            return user;
        }

        public string GetSubreddit()
        {
            return subreddit;
        }

        public bool Equals(Post? other)
        {
            if (other == null)
            {
                return false;
            }

            return (id.Equals(other.GetId()));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Post);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}

