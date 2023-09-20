namespace RedditConsumer.Models.Reddit
{
	public class SubredditPost
	{
        public required double Created { get; set; }
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required int Score { get; set; }
        public required string Author { get; set; }
	}
}

