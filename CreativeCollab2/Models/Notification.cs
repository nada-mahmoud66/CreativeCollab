using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class Notification
{
	public int NotificationId { get; set; }

	public string? UserId { get; set; }
	public string? SenderId { get; set; }
	public string? SenderName { get; set; }
	public string? SenderImg { get; set; }

	public string? NotificationType { get; set; }
	public string NotificationMessage { get; set; }
	public int? PostId { get; set; }
	public int? GroupId { get; set; }

	public bool? IsRead { get; set; }

	public DateTime? NotificationDateTime { get; set; }

	public virtual Post? Post { get; set; }

	public virtual ApplicationUser? User { get; set; }
}
