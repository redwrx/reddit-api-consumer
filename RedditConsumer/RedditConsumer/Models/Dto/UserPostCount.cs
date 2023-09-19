using System;
namespace RedditConsumer.Models.Dto
{
	public class UserPostCount
	{
		public required string Username { get; set; }
		public int PostCount { get; set; }
	}
}

