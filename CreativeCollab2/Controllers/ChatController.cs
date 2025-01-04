using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace CreativeCollab2.Controllers
{
	public class ChatController : Controller
	{
		private readonly ILogger<ChatController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		//CreativeCollabContext ctx = new CreativeCollabContext();
		Ilikes ClsLikes;
		Iposts ClsUserPosts;
		Iintersts ClsIntersts;
		Icomments ClsComments;
		Ifollow ClsFollow;
		Igroups ClsGroups;
		IChats ClsChats;

		public ChatController(IChats chat, Igroups g, Ifollow i, Icomments c, Ilikes l, Iintersts interst, Iposts post, ILogger<ChatController> logger, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
			ClsUserPosts = post;
			ClsIntersts = interst;
			ClsComments = c;
			ClsLikes = l;
			ClsFollow = i;
			ClsGroups = g;
			ClsChats = chat;
		}
		public async void FillViewBags(ApplicationUser user)
		{

			ViewBag.Groups = ClsGroups.GetAllGroupsByUserId(user.Id);
			ViewBag.MyGroups = ClsGroups.GetOwnGroups(user.Id);
			ViewBag.ProfileImage = user.ProfileImage;
			ViewBag.CoverImage = user.CoverImage;
			ViewBag.NumOfFollowers = user.Followers.Count;
			ViewBag.FirstName = user.FirstName;
			ViewBag.LastName = user.LastName;
			ViewBag.meID = user.Id;
			ViewBag.Notifications = user.Notifications;
		}
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			List<InboxWithUnreadMessage> inboxes = new List<InboxWithUnreadMessage>();
			inboxes = await ClsChats.getInboxIds(user.Id);
			user = await _userManager.Users.Include(u => u.Followers).Include(u => u.Followings).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefaultAsync(u => u.Id == user.Id);
			FillViewBags(user);
			return View(new InboxList() { Inbox = inboxes, User = user });
		}
		[HttpPost]
		public async Task<IActionResult> SendMessage(string recieverId, string message, string senderId)
		{

			if (ClsChats.send(senderId, message, recieverId))
			{
				return Json(new { success = true });
			}
			else
			{
				return StatusCode(500, "An error occurred while send the message");
			}
		}
		[HttpPost]
		public async Task<IActionResult> SendShareMessage(string recieverId, int postId, string senderId)
		{

			if (ClsChats.sendShare(senderId, postId, recieverId))
			{
				return Json(new { success = true });
			}
			else
			{
				return StatusCode(500, "An error occurred while send the shared message");
			}
		}
		public async Task<JsonResult> GetMessages(string receiverId, string senderId)
		{
			try
			{
				var messages = ClsChats.getMessages(senderId, receiverId);
				int flag = 0;
				if (ClsChats.cheakBlock(senderId, receiverId) || ClsChats.cheakIBlocked(senderId, receiverId)) // Corrected method name
				{
					flag = 1;
				}

				var response = new { messages, flag };
				return Json(response);
			}
			catch (Exception ex)
			{

				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> readMessage(string recieverId, string senderId)
		{

			if (ClsChats.readMessage(senderId, recieverId))
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, "An error occurred read message.");
			}
		}

	}
}
