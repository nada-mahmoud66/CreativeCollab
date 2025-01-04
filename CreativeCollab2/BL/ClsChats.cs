using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CreativeCollab2.BL
{
	public class Message
	{
		public string text { get; set; }
		public string flag { get; set; }
		public string image { get; set; }
		public DateTime? MessageDateTime { get; set; }

	}

	public class MessageNotification
	{
		public string id { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string type { get; set; }

		public bool? flag { get; set; }
		public string? lastMessage { get; set; }
		public string image { get; set; }
		public string? messageDuration { get; set; }
		public DateTime? messageDateTime { get; set; }


	}
	public interface IChats
	{
		public bool send(string userId, string text, string reciever);
		public bool sendShare(string userId, int postId, string reciever);


		public List<Message> getMessages(string userId, string reciever);
		public bool readMessage(string userId, string reciever);
		public bool cheakBlock(string userId, string reciever);
		public bool cheakIBlocked(string userId, string reciever);




		public Task<List<InboxWithUnreadMessage>> getInboxIds(string userId);
		public Task<List<MessageNotification>> getMessageNotfications(string userId);



	}
	public class ClsChats : IChats
	{
		CreativeCollabContext ctx;
		private readonly UserManager<ApplicationUser> _userManager;

		public ClsChats(CreativeCollabContext _ctx, UserManager<ApplicationUser> userManager)
		{
			ctx = _ctx;
			_userManager = userManager;

		}

		public bool send(string userId, string text, string reciever)
		{
			try
			{
				ChatMessage message = new ChatMessage()
				{

					SenderId = userId,
					ReceiverId = reciever,
					MessageDateTime = DateTime.UtcNow,
					MessageText = text,
					MessageType = "text",


				};

				ctx.Add(message);
				ctx.SaveChanges();
				return true;

			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public List<Message> getMessages(string userId, string reciever)
		{
			var flag = "you";
			var photo = "";
			List<Message> messages = new List<Message>();
			List<ChatMessage> temp = ctx.ChatMessages.Where(m => ((m.SenderId == userId) && (m.ReceiverId == reciever)) || ((m.SenderId == reciever) && (m.ReceiverId == userId))).ToList();
			foreach (ChatMessage message in temp)
			{


				if (message.SenderId == userId)
				{
					flag = "me";
					photo = ctx.Users.FirstOrDefault(i => i.Id == userId).ProfileImage;
				}
				else
				{
					message.IsRead = true;
					flag = "you";
					photo = ctx.Users.FirstOrDefault(i => i.Id == reciever).ProfileImage;

				}
				messages.Add(new Message
				{
					text = message.MessageText,
					flag = flag,
					MessageDateTime = message.MessageDateTime,
					image = photo,
				});

			}
			ctx.SaveChanges();
			return messages.OrderBy(m => m.MessageDateTime).ToList();
		}
		public async Task<List<InboxWithUnreadMessage>> getInboxIds(string userId)
		{
			List<ChatMessage> temp = ctx.ChatMessages
				.Where(m => ((m.SenderId == userId) || (m.ReceiverId == userId)))
				.ToList();
			int flag = 0;
			InboxList inboxList = new InboxList();
			foreach (ChatMessage message in temp)
			{
				string otherUserId = message.SenderId == userId ? message.ReceiverId : message.SenderId;
				ApplicationUser otherUser = await _userManager.FindByIdAsync(otherUserId);
				if (otherUser != null)
				{
					if (cheakBlock(userId, otherUserId))
					{
						flag = 1;
					}
					inboxList.Inbox.Add(new InboxWithUnreadMessage { User = otherUser, flag = flag });
				}
			}

			var user = await _userManager.Users
				.Include(u => u.Followers)
				.Include(u => u.Followings)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user != null)
			{
				var followersInbox = user.Followers.Select(u => new InboxWithUnreadMessage { User = u }).ToList();
				var followingsInbox = user.Followings.Select(u => new InboxWithUnreadMessage { User = u }).ToList();
				inboxList.Inbox.Concat(followersInbox);
				inboxList.Inbox.Concat(followingsInbox);
				inboxList.Inbox = inboxList.Inbox.Distinct().ToList();
				List<InboxWithUnreadMessage> inboxWithUnreadMessages = new List<InboxWithUnreadMessage>();

				inboxWithUnreadMessages = inboxList.Inbox.Select(u => new InboxWithUnreadMessage
				{
					User = u.User,
					flag = u.flag,
					NumberOfUnreadMessages = temp.Count(m => (m.ReceiverId == userId && m.SenderId == u.User.Id) && m.IsRead == false)
				}).OrderByDescending(u => temp
					.Where(m => (m.SenderId == u.User.Id || m.ReceiverId == u.User.Id))
					.Max(m => m.MessageDateTime))
			   .ToList();

				return inboxWithUnreadMessages.Distinct().ToList();
			}

			return new List<InboxWithUnreadMessage>();
		}



		public async Task<List<MessageNotification>> getMessageNotfications(string userId)
		{
			List<MessageNotification> notifications = new List<MessageNotification>();
			List<ChatMessage> temp = ctx.ChatMessages.Include(u => u.Sender)
				.Where(m => m.ReceiverId == userId && m.IsRead == false)
				.OrderByDescending(m => m.MessageDateTime)
				.ToList();

			Dictionary<string, (string, DateTime?)> lastMessages = new Dictionary<string, (string, DateTime?)>();

			foreach (ChatMessage message in temp)
			{

				if (lastMessages.ContainsKey(message.SenderId))
				{
					lastMessages[message.SenderId] = (message.MessageText, message.MessageDateTime);
				}
				else
				{
					lastMessages.Add(message.SenderId, (message.MessageText, message.MessageDateTime));
					MessageNotification notification = new MessageNotification
					{
						id = message.SenderId,
						firstName = message.Sender.FirstName,
						lastName = message.Sender.LastName,
						image = message.Sender.ProfileImage,
						type = message.MessageType

					};
					notifications.Add(notification);
				}
			}
			foreach (var key in lastMessages.Keys.ToList())
			{

				var notification = notifications.FirstOrDefault(n => n.id == key);
				if (notification != null)
				{
					var (messageText, messageDateTime) = lastMessages[key];
					if (notification.lastMessage == null)
					{
						if (notification.type == "share")
						{
							notification.lastMessage = "shared a post with you";

						}
						else
						{
							notification.lastMessage = messageText;
						}
					}
					if (notification.messageDuration == null)
					{
						TimeSpan? difference = DateTime.UtcNow - messageDateTime;
						int durationInMinutes = (int)Math.Floor(difference?.TotalMinutes ?? 0);
						notification.messageDuration = FormatDuration(durationInMinutes);

					}
				}
			}

			return notifications;
		}
		public static string FormatDuration(int durationInMinutes)
		{
			if (durationInMinutes < 0)
			{
				return "Invalid duration";
			}

			int years = durationInMinutes / (60 * 24 * 365);
			int months = durationInMinutes / (60 * 24 * 30) % 12;
			int days = durationInMinutes / (60 * 24) % 30;
			int hours = durationInMinutes / 60 % 24;
			int minutes = durationInMinutes % 60;

			List<string> parts = new List<string>();
			if (years > 0)
			{
				parts.Add($"{years} year{(years != 1 ? "s" : "")}");
			}
			else if (months > 0)
			{
				parts.Add($"{months} month{(months != 1 ? "s" : "")}");
			}
			else if (days > 0)
			{
				parts.Add($"{days} day{(days != 1 ? "s" : "")}");
			}
			else if (hours > 0)
			{
				parts.Add($"{hours} hour{(hours != 1 ? "s" : "")}");
			}
			else if (minutes > 0)
			{
				parts.Add($"{minutes} minute{(minutes != 1 ? "s" : "")}");
			}

			else if (parts.Count == 0)
			{
				return "Less than a minute";
			}

			return string.Join(", ", parts);
		}
		public bool readMessage(string userId, string reciever)
		{
			try
			{
				ChatMessage message = ctx.ChatMessages
			   .Where(m => (m.SenderId == userId) && (m.ReceiverId == reciever))
			   .OrderByDescending(m => m.MessageDateTime)
			   .FirstOrDefault();



				message.IsRead = true;

				ctx.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool sendShare(string userId, int postId, string reciever)
		{
			try
			{
				ChatMessage message = new ChatMessage()
				{


					SenderId = userId,
					ReceiverId = reciever,
					MessageDateTime = DateTime.UtcNow,
					MessageText = " <a style='color: blue;' href='/UserProfile/Post?postId=" + postId + "'>" + "Check out this link" + "</a>",
					MessageType = "share",


				};

				ctx.Add(message);
				ctx.SaveChanges();
				return true;

			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool cheakBlock(string userId, string reciever)
		{
			try
			{
				var user = ctx.Users.Include(u => u.BlockedUsers).FirstOrDefault(u => u.Id == userId);
				if (user.BlockedUsers.Select(u => u.Id).Contains(reciever))
				{
					return true;
				}
				else
				{
					return false;
				}


			}
			catch (Exception ex)
			{
				return false;
			}

		}

		public bool cheakIBlocked(string userId, string reciever)
		{
			try
			{
				var user = ctx.Users.Include(u => u.BlockedUsers).FirstOrDefault(u => u.Id == reciever);
				if (user.BlockedUsers.Select(u => u.Id).Contains(userId))
				{
					return true;
				}
				else
				{
					return false;
				}


			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}

}
