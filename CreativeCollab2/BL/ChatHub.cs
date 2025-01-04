using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CreativeCollab2.BL
{
	public class ChatHub : Hub
	{

		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public async Task SendMessageToUser(string receiverUserId, string message, string senderId)
		{


			await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderId, message);
		}
	}

}