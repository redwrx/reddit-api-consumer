using RedditConsumer.Models;

namespace RedditConsumer.Repositories.InMemory
{
	public class UserRepository: IUserRepository
	{
        private static readonly List<User> users = new();

        public UserRepository()
		{
		}

        public User Add(User user)
        {
            if (!users.Contains(user))
            {
                users.Add(user);
            }

            return user;
        }

        public User GetById(string id) => users.Find(u => u.GetId().Equals(id, StringComparison.Ordinal));
    }
}

