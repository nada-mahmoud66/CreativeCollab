namespace CreativeCollab2.Models
{
	public class VmGroupMembersForAdmin:VmGroupAdmin
	{
        public List<ApplicationUser> MutualFriends { get; set; }
		public List<ApplicationUser> RestOfGroupFollowers { get; set; }
    }
}
