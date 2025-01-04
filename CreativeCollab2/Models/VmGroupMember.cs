namespace CreativeCollab2.Models
{
	public class VmGroupMember
	{
        public Group group { get; set; }
        public bool isMember { get; set; }
        public List<ApplicationUser> FriendsToInvite { get; set; }
        public int NoOfMembers { get; set; }
        public ApplicationUser CurrentUser { get; set; }
		public bool isFollowingAdmin { get; set; }
	}
}
