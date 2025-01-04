using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.Controllers
{
	public class TimeLineController : Controller
	{
		private readonly ILogger<TimeLineController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		//CreativeCollabContext ctx = new CreativeCollabContext();
		Iposts ClsUserPosts;
		Iintersts ClsIntersts;
		Ilikes ClsLikes;
		Icomments ClsComments;
		Ifollow ClsFollow;

		IChats ClsChats;
		vmCompoUserAndPosts vmCompo = new vmCompoUserAndPosts();

		vmCompoUserInterst vmCompoUserInterst = new vmCompoUserInterst();
		public TimeLineController(IChats cd, Ifollow i, Icomments c, Ilikes l, Iintersts interst, Iposts post, ILogger<TimeLineController> logger, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
			ClsUserPosts = post;
			ClsIntersts = interst;
			ClsFollow = i;
			ClsLikes = l;
			ClsChats = cd;

		}


		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			//user = await _userManager.Users.Include(u => u.Followers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == user.Id);

			var posts = await ClsUserPosts.GetTimeLinePostsByID(user.Id);
			user = await _userManager.Users.Include(u => u.BlockedUsers).Include(u => u.BlockingUsers).Include(u => u.Followers).Include(u => u.Followings).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefaultAsync(u => u.Id == user.Id);
			ViewBag.ProfileImage = user.ProfileImage;
			ViewBag.CoverImage = user.CoverImage;
			ViewBag.NumOfFollowers = user.Followers.Count;
			ViewBag.FirstName = user.FirstName;
			ViewBag.LastName = user.LastName;
			ViewBag.Id = user.Id;
			ViewBag.Followings = user.Followings.ToList();
			ViewBag.Notifications = user.Notifications;
			ViewBag.user = user;
			ViewBag.meID = user.Id;
			ViewBag.titels = ClsUserPosts.getTitleList(posts);

			ViewBag.likesCount = ClsLikes.GetTotalLikesPerWeek(user.Id);

			vmTimeLine tl = new vmTimeLine();
			tl.MyPosts = posts;

			// for model lolo : ClsUserPosts.CheckLikes(user.Id)  // حطيها بدل الفولس
			if (ClsUserPosts.CheckLikes(user.Id))
			{


				RecommendationModel rc = new RecommendationModel();
				List<int> ids = new List<int>();
				ids = await rc.recommend(user.Id);
				List<Post> recPosts = new List<Post>();
				recPosts = ClsUserPosts.recPosts(ids, user.Id);
				tl.RecommendationPosts = recPosts;
				ViewBag.titels2 = ClsUserPosts.getTitleList(recPosts);

			}
			else
			{
				List<Post> recPosts = new List<Post>();
				recPosts = ClsUserPosts.getNewUserDiscovery(user.Id);
				foreach (Post post in recPosts)
				{
					if (user.BlockedUsers.Select(u => u.Id).Contains(post.UserId) || post.User.BlockedUsers.Select(u => u.Id).Contains(user.Id))
					{
						recPosts.Remove(post);
					}
				}
				ViewBag.titels2 = ClsUserPosts.getTitleList(recPosts);
				tl.RecommendationPosts = recPosts;

			}


			return View(tl);
		}
		[HttpPost]
		public async Task<IActionResult> getMessageNotfications(string userId)
		{
			var notifications = await ClsChats.getMessageNotfications(userId);
			return Json(notifications);
		}

		[HttpPost]
		public async Task<IActionResult> AddLike(int postId)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (await ClsLikes.AddLike(postId, userId))
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, "An error occurred while adding the like.");
			}
		}
		[HttpPost]
		public async Task<IActionResult> RemoveLike(int postId)
		{

			ApplicationUser user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (await ClsLikes.RemoveLike(postId, userId))
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, "An error occurred while removing the like.");
			}

		}
		[HttpPost]
		public IActionResult RemovePost(int postId)
		{
			if (ClsUserPosts.RemovePost(postId))
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, "An error occurred while removing the Post.");
			}

		}
		[HttpPost]
		public async Task<IActionResult> AddPost(string enteredText, IFormFile uploadedImage, DateTime Date)
		{
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (await ClsUserPosts.AddPost(enteredText, userId, uploadedImage, Date, null))
			{
				var lastId = ClsUserPosts.GetLastPostId();
				var post = ClsUserPosts.getPost(lastId);
				var titel = ClsUserPosts.getTitle(post);
				return Json(new { postID = lastId, titels = titel });

			}
			else
			{
				return StatusCode(500, "An error occurred while adding the post");
			}

		}
		[HttpPost]
		public async Task<IActionResult> UpdatePost(string NewText, int postId)
		{
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			if (ClsUserPosts.UpdatePost(NewText, postId))
			{
				return Ok();
			}
			else
			{
				return StatusCode(500, "An error occurred while updating the post");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddComment(int postId, string text)
		{

			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;


			if (ClsComments.AddComment(postId, text, userId))
			{

				var commentCount = ClsComments.GetCommentsCount(postId);
				return Json(new { commentCount = commentCount });

			}
			else
			{

				return StatusCode(500, "An error occurred while adding the comment.");
			}
		}
		[HttpPost]
		public async Task<IActionResult> getUserNotification(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			var data = new UserInfo
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				ProfileImage = user.ProfileImage
			};

			return Json(data);
		}

	}
}
