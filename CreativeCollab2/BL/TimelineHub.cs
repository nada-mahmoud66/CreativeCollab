using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CreativeCollab2.BL
{
	public class TimelineHub : Hub
	{
		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}

		public async Task SendPostToTimeline(string senderId, List<string> receiversIds, int postId, string UserImg, string Username, DateTime time, string content, string titles, string postImg, int? groupId, string? groupName)
		{
			await Clients.Users(receiversIds).SendAsync("ReceivePostInTimelne", senderId, postId, UserImg, Username, time, content, titles, postImg, groupId, groupName);
		}
		public async Task SendPostToGroup(string senderId, int postId, string UserImg, string Username, DateTime time, string content, string titles, string postImg, int groupId, string? groupName)
		{
			await Clients.All.SendAsync("ReceivePostInGroup", senderId, postId, UserImg, Username, time, content, titles, postImg, groupId, groupName);
		}

	}
}
