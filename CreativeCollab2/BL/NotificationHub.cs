using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CreativeCollab2.BL
{
	public class NotificationHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}

		public async Task SendNotification(string senderId, string receiverId, string message, string type,int? postId, string img,string name,DateTime time, int? groupId, int notificationId)
		{
			await Clients.User(receiverId).SendAsync("ReceiveNotification", senderId, receiverId, type, message, postId, img,name,time, groupId, notificationId);
		}
		public async Task UnSendNotification(int notificatioId, bool isRead, string receiverId)
		{
			await Clients.User(receiverId).SendAsync("UnReceiveNotification", notificatioId,isRead, receiverId);
		}
	}
}
