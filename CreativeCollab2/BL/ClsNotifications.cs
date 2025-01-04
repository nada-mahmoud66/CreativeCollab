using CreativeCollab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using static CreativeCollab2.BL.ClsNotifications;

namespace CreativeCollab2.BL
{
	public interface INotifications
	{
		public  Task Send(string senderId, string receiverId,  string message, string type, int? postId, string img, string name, int? groupId);
		public Task Unsend(int? postId, string userId, string type);
		public void ReadNotifications(string userId);
	}
	public class ClsNotifications : INotifications
	{
			private readonly CreativeCollabContext _context;
			private readonly IHubContext<NotificationHub> _hubContext;

			public ClsNotifications(CreativeCollabContext ctx, IHubContext<NotificationHub> hubContext)
			{
				_context = ctx;
				_hubContext = hubContext;
			}

		public void ReadNotifications(string userId)
		{
			var userNotifications = _context.Notifications.Where(n => n.UserId == userId).ToList();
			foreach(var notification in userNotifications)
			{
				notification.IsRead = true;
				try
				{
					_context.SaveChanges();
				}
				catch(Exception ex) { }
			}
		}

		public async Task Send(string senderId, string receiverId, string message, string type, int? postId, string img,string name,int? groupId)
			{
			try
			{
				var notification = new Notification
				{
					UserId = receiverId,
					SenderId = senderId,
					NotificationMessage = message,
					NotificationType = type,
					NotificationDateTime = DateTime.Now,
					PostId = postId,
					GroupId = groupId,
					SenderName = name,
					SenderImg = img
				};
				_context.Add(notification);
				_context.SaveChanges();
				await _hubContext.Clients.User(receiverId).SendAsync("ReceiveNotification", senderId, receiverId, type, message, postId, img, name, DateTime.Now,groupId,notification.NotificationId);
			}
			catch (Exception ex) { }
		}

		public async Task Unsend(int? postId, string userId, string type)
		{
			try
			{
				var notification = _context.Notifications.Where(n=> n.NotificationType == type && n.SenderId == userId && n.PostId==postId).FirstOrDefault();
				_context.Remove(notification);
				_context.SaveChanges();
				await _hubContext.Clients.User(notification.UserId).SendAsync("UnReceiveNotification", notification.NotificationId,notification.IsRead, notification.UserId);
			}
			catch (Exception ex) { }
		}
	}
	
}
