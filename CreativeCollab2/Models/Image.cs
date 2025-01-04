using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Image
{
	public int ImageId { get; set; }

	public string? ImageName { get; set; }

	public int? InterestId { get; set; }

	public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

	public virtual Interest? Interest { get; set; }

	public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
