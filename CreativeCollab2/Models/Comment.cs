using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Comment
{
	public int CommentId { get; set; }

	public int? ParentCommentId { get; set; }

	public string? UserId { get; set; }

	public int? PostId { get; set; }

	public int? ImageId { get; set; }

	public string? CommentText { get; set; }

	public DateTime? CommentDate { get; set; }

	public bool? IsDeleted { get; set; }

	public virtual Image? Image { get; set; }

	public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

	public virtual Comment? ParentComment { get; set; }

	public virtual Post? Post { get; set; }

	public virtual ApplicationUser? User { get; set; }
}
