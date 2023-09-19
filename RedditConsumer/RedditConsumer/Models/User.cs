using System;
namespace RedditConsumer.Models
{
	public class User : IEquatable<User>
    {
        private readonly string id;
        private readonly string username;

        public User(string username)
        {
            this.id = Guid.NewGuid().ToString();
            this.username = username;
        }

        public bool Equals(User? other)
        {
            if (other == null)
            {
                return false;
            }

            return (id.Equals(other.GetId()));
        }

        public string GetId()
        {
            return id;
        }

        public string GetUsername()
        {
            return username;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}

