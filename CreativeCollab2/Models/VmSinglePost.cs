namespace CreativeCollab2.Models
{
	public class VmSinglePost
	{
        public ApplicationUser ApplicationUser { get; set; }
        public Post post { get; set; }
		public bool IsLikedByCurrentUser { get; set; }
	}
}
