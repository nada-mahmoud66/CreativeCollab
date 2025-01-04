using CreativeCollab2.BL;
using System.ComponentModel.DataAnnotations;

namespace CreativeCollab2.Models
{
	public class InboxWithUnreadMessage
	{

		public ApplicationUser User { get; set; }
		public int NumberOfUnreadMessages { get; set; }
		public int flag = 0;

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (InboxWithUnreadMessage)obj;
			return this.User.Id == other.User.Id;
		}

		public override int GetHashCode()
		{
			if (this.User == null)
			{
				return 0; // or any other constant value, depending on your requirements
			}

			return this.User.Id.GetHashCode();
		}

	}

	public class InboxList
	{
		public InboxList()
		{

			Inbox = new List<InboxWithUnreadMessage>();

		}
		public int flag = 0;
		public virtual ICollection<InboxWithUnreadMessage> Inbox { get; set; }

		public ApplicationUser User { get; set; }
	}
}
