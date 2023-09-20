using RedditConsumer.Models;

namespace RedditConsumer.Repositories.InMemory
{
	public class UserRepository: IUserRepository
	{
        private readonly List<User> users = new();

        public UserRepository()
		{
		}

        public User Add(User user)
        {
            var existing = users.Where(u => u.GetUsername() == user.GetUsername()).FirstOrDefault();
            if (existing == null)
            {
                users.Add(user);
            }
            else
            {
                user = existing;
            }

            return user;
        }

        public User GetById(string id) => users.Find(u => u.GetId().Equals(id, StringComparison.Ordinal));
    }
}

