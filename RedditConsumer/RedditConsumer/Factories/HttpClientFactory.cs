namespace RedditConsumer.Factories
{
	public class HttpClientFactory : IHttpClientFactory
	{
		public HttpClientFactory()
		{
		}

        public HttpClient Build()
        {
            return new HttpClient();
        }
    }
}

