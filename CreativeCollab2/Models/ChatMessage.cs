using System;
using System.Collections.Generic;

namespace CreativeCollab2.Models;

public partial class ChatMessage
{
	public int MessageId { get; set; }

	public string? SenderId { get; set; }

	public string? ReceiverId { get; set; }

	public string? MessageText { get; set; }

	public string? MessageType { get; set; }

	public DateTime? MessageDateTime { get; set; }

	public bool? IsRead { get; set; }

	public virtual ApplicationUser? Receiver { get; set; }

	public virtual ApplicationUser? Sender { get; set; }
}
