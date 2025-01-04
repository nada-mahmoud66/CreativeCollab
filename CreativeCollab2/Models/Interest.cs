using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Interest
{
	public int InterestId { get; set; }

	public string? InterestName { get; set; }

	public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

	public virtual ICollection<Image> Images { get; set; } = new List<Image>();

	public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
