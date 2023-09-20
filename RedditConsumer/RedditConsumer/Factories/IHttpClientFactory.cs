using System;
namespace RedditConsumer.Factories
{
	public interface IHttpClientFactory
	{
		HttpClient Build();
	}
}

