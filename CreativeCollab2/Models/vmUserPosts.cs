namespace CreativeCollab2.Models
{
	public class vmUserPosts
	{

		public int PostID { get; set; }
		public int? GroupID { get; set; }
		public string PostText { get; set; }
		public string PostImage { get; set; }
		public bool IsLikedByCurrentUser { get; set; }
		public DateTime PostDateTime { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Like> Likes { get; set; }
		public string? Titles { get; set; }
	}
}
