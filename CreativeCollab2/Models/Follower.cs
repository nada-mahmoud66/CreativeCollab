namespace CreativeCollab2.Models
{
	public class Follower
	{
		public string FollowerId { get; set; }
		public string FollowingId { get; set; }

		public virtual ApplicationUser Follower_ { get; set; }
		public virtual ApplicationUser Following { get; set; }
	}
}
