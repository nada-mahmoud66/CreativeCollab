using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CreativeCollab2.Controllers
{
	public class ProfileController : Controller
	{


		private readonly ILogger<ProfileController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		//	CreativeCollabContext ctx = new CreativeCollabContext();
		Iposts ClsUserPosts;
		Iintersts ClsIntersts;
		Ilikes ClsLikes;
		Icomments ClsComments;
		Ifollow ClsFollow;
		IChats ClsChats;
		public ProfileController(IChats c, Ifollow i, Ilikes l, Iintersts interst, Iposts post, ILogger<ProfileController> logger, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
			ClsUserPosts = post;
			ClsIntersts = interst;
			ClsFollow = i;
			ClsLikes = l;
			ClsChats = c;
		}

		public async Task<IActionResult> Index(string id)
		{
			var me = await _userManager.GetUserAsync(User);
			me = _userManager.Users.Include(u => u.Followings).Include(u => u.Followers).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefault(u => u.Id == me.Id);

			if (me.Id == id)
			{
				return RedirectToAction("Index", "UserProfile");
			}

			var posts = new List<Post>();
			posts = await ClsUserPosts.GetProfileUserPostsByID(id);



			ViewBag.me = me;
			ViewBag.Flag = me.Followings.Select(u => u.Id).ToList().Contains(id);

			var him = _userManager.Users.Include(I => I.Interests).Include(u => u.Followings).Include(u => u.Followers).FirstOrDefault(u => u.Id == id);

			bool temp1 = me.Followings.Select(u => u.Id).ToList().Contains(id);
			bool temp2 = him.Followings.Select(u => u.Id).ToList().Contains(me.Id);
			if (temp1 && temp2)
			{
				ViewBag.ShowFlag = true;
			}
			else
			{
				ViewBag.ShowFlag = false;
			}
			if (ClsChats.cheakBlock(me.Id, him.Id))
			{
				ViewBag.block = 1;
			}
			else
			{
				if (ClsChats.cheakIBlocked(me.Id, him.Id))
				{
					ViewBag.block = 2;
				}
				else
				{
					ViewBag.block = 0;
				}

			}
			ViewBag.ProfileImage = me.ProfileImage;
			ViewBag.him = him;
			ViewBag.meID = me.Id;
			ViewBag.Notifications = me.Notifications;

			return View(posts);
		}
		[HttpPost]
		public async Task<IActionResult> addFollow(string FollowingId)
		{
			var me = await _userManager.GetUserAsync(User);
			var userId = me.Id;

			if (await ClsFollow.AddFollowing(FollowingId, userId))
			{
				var following = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == FollowingId);

				return Json(new { Success = true, ImageName = following.ProfileImage, FirstName = following.FirstName, LastName = following.LastName });
			}
			else
			{
				return StatusCode(500, "An error occurred while adding the follow.");

			}
		}
		[HttpPost]
		public async Task<IActionResult> blockUser(string userId)
		{
			var me = await _userManager.GetUserAsync(User);
			var meId = me.Id;
			if (await ClsFollow.BlockUser(meId, userId))
			{
				return Json(new { Success = true });
			}
			else
			{
				return StatusCode(500, "An error occurred while blocking");

			}


		}
		[HttpPost]
		public async Task<IActionResult> unBlockUser(string userId)
		{
			var me = await _userManager.GetUserAsync(User);
			var meId = me.Id;
			if (await ClsFollow.UnBlockUser(meId, userId))
			{
				return Json(new { Success = true });
			}
			else
			{
				return StatusCode(500, "An error occurred while blocking");

			}


		}
	}
}
