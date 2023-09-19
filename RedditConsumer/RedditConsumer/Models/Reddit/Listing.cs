namespace RedditConsumer.Models.Reddit
{
	public class Listing<T>
	{
        public required ListingData<T> Data { get; set; }
    }
}

