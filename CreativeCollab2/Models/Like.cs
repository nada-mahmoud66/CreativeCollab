using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Like
{
	public int LikeId { get; set; }

	public int? PostId { get; set; }

	public string? UserId { get; set; }

	public DateTime? LikeDate { get; set; }

	public virtual Post? Post { get; set; }

	public virtual ApplicationUser? User { get; set; }
}
