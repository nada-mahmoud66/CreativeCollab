using Microsoft.AspNetCore.Identity;

namespace CreativeCollab2.Models
{
	public class ApplicationUser : IdentityUser
	{

		public ApplicationUser()
		{
			ChatMessageReceivers = new List<ChatMessage>();
			ChatMessageSenders = new List<ChatMessage>();
			Comments = new List<Comment>();
			Groups = new List<Group>();
			Likes = new List<Like>();
			Notifications = new List<Notification>();
			Posts = new List<Post>();
			BlockedUsers = new List<ApplicationUser>();
			BlockingUsers = new List<ApplicationUser>();
			Followers = new List<ApplicationUser>();
			Followings = new List<ApplicationUser>();
			GroupsNavigation = new List<Group>();
			Interests = new List<Interest>();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDate { get; set; }
		public string ProfileImage { get; set; }
		public string CoverImage { get; set; }
		public char Gender { get; set; }

		// Add navigation properties for your relationships
		public virtual ICollection<ChatMessage> ChatMessageReceivers { get; set; }
		public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
		public virtual ICollection<Group> Groups { get; set; }
		public virtual ICollection<Like> Likes { get; set; }
		public virtual ICollection<Notification> Notifications { get; set; }
		public virtual ICollection<Post> Posts { get; set; }
		public virtual ICollection<ApplicationUser> BlockedUsers { get; set; }
		public virtual ICollection<ApplicationUser> BlockingUsers { get; set; }
		public virtual ICollection<ApplicationUser> Followers { get; set; }
		public virtual ICollection<ApplicationUser> Followings { get; set; }
		public virtual ICollection<Group> GroupsNavigation { get; set; }
		public virtual ICollection<Interest> Interests { get; set; }

	}
}
