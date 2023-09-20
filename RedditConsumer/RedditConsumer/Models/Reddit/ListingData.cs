namespace RedditConsumer.Models.Reddit
{
	public class ListingData<T>
	{
        public required List<ListingChild<T>> Children { get; set; }
        public required string After { get; set; }
    }
}