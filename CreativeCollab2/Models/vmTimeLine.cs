namespace CreativeCollab2.Models
{
	public class vmTimeLine
	{
		public vmTimeLine()
		{
			RecommendationPosts = new List<Post>();
			MyPosts = new List<Post>();
		}
		public List<Post> MyPosts { get; set; }

		public List<Post> RecommendationPosts { get; set; }

	}
}
