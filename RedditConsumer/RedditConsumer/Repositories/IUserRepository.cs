using System;
using RedditConsumer.Models;

namespace RedditConsumer.Repositories
{
	public interface IUserRepository
	{
		User GetById(string id);
		User Add(User user);
	}
}

