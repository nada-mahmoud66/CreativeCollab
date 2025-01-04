namespace CreativeCollab2.Models
{
    public class VmGroupAdmin
    {
        public Group group { get; set; }
        public List<ApplicationUser> FriendsToInvite { get; set; }
        public int NoOfMembers { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public IFormFile coverImageFile { get; set; }

	}
}
