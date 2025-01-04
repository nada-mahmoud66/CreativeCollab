namespace CreativeCollab2.Models
{
	public class VmGroupMembersForMember:VmGroupMember
	{
        public bool isFollowingAdmin { get; set; }
        public List<ApplicationUser> MutualFriends { get; set; }
		public List<ApplicationUser> RestOfGroupFollowers { get; set; }
    }
}
