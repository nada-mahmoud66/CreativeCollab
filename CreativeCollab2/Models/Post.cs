using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Post
{
	public int PostId { get; set; }

	public string? UserId { get; set; }

	public int? GroupId { get; set; }

	public string? PostText { get; set; }

	public int? PostImageId { get; set; }

	public DateTime? PostDateTime { get; set; }

	public bool? IsDeleted { get; set; }

	public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

	public virtual Group? Group { get; set; }

	public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

	public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

	public virtual Image? PostImage { get; set; }

	public virtual ApplicationUser? User { get; set; }

	//public string? Titles { get; set; }

}
