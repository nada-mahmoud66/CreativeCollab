using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Group
{
	public int GroupId { get; set; }

	public string? GroupName { get; set; }

	public string? Description { get; set; }

	public string? CreatorUserId { get; set; }

	public int? InterestId { get; set; }

	public string? CoverImage { get; set; }

	public DateTime? CreatedDateTime { get; set; }

	public bool? IsDeleted { get; set; }

	public virtual ApplicationUser? CreatorUser { get; set; }

	public virtual Interest? Interest { get; set; }

	public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

	public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
