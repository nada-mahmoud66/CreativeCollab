using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CreativeCollab2.Controllers
{
	public class UserProfileController : Controller
	{
		private readonly ILogger<UserProfileController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		//CreativeCollabContext ctx = new CreativeCollabContext();
		Ilikes ClsLikes;
		Iposts ClsUserPosts;
		Iintersts ClsIntersts;
		Icomments ClsComments;
		Ifollow ClsFollow;
		Igroups ClsGroups;
		INotifications ClsNotifications;


		vmCompoUserInterst vmCompoUserInterst = new vmCompoUserInterst();
		public UserProfileController(INotifications n, Igroups g, Ifollow i, Icomments c, Ilikes l, Iintersts interst, Iposts post, ILogger<UserProfileController> logger, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
			ClsUserPosts = post;
			ClsIntersts = interst;
			ClsComments = c;
			ClsLikes = l;
			ClsFollow = i;
			ClsGroups = g;
			ClsNotifications = n;
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
			ViewBag.Notifications = user.Notifications;
			ViewBag.meID = user.Id;
			//ViewBag.Followings = user.Followings;
		}
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			user = await _userManager.Users.Include(u => u.Followers)
				.Include(u => u.Followings)
				.Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime))
				.FirstOrDefaultAsync(u => u.Id == user.Id);
			var userId = user.Id;
			vmCompoUserAndPosts vmCompo = new vmCompoUserAndPosts();

			FillViewBags(user);
			ViewBag.Interests = ClsIntersts.GetAllIntersts();
			vmCompo.ApplicationUser = user;
			var userPosts = await ClsUserPosts.GetProfileUserPostsByID(userId);
			ViewBag.titels = ClsUserPosts.getTitleList(userPosts);
			if (userPosts.Count != 0)
			{
				//fill the view model
				var userPostsViewModel = userPosts.Select(p => new vmUserPosts
				{
					PostID = p.PostId,
					PostText = p.PostText,
					PostImage = p.PostImage.ImageName,
					PostDateTime = (DateTime)p.PostDateTime,
					GroupID = p.GroupId,
					Comments = p.Comments.OrderByDescending(c => c.CommentDate).ToList(),
					//Titles = p.Titles,
					Likes = p.Likes.ToList(),
				}).OrderByDescending(a => a.PostDateTime).ToList();
				// likes .
				foreach (var userPost in userPostsViewModel)
				{

					foreach (var like in userPost.Likes)
					{
						if (like.UserId == userId)
						{
							userPost.IsLikedByCurrentUser = true;
							break;
						}
						else
						{
							userPost.IsLikedByCurrentUser = false;
						}

					}

				}
				vmCompo.vmUserPosts = userPostsViewModel;
				return View(vmCompo);
			}
			else
			{
				vmCompo.vmUserPosts = new List<vmUserPosts>();
			}
			return View(vmCompo);
		}
		[HttpPost]
		public async Task<IActionResult> ReadNotifications()
		{
			var user = await _userManager.GetUserAsync(User);

			ClsNotifications.ReadNotifications(user.Id);
			return Ok();
		}
		public async Task<IActionResult> Post(int postId)
		{

			var user = await _userManager.GetUserAsync(User);
			user = await _userManager.Users
				.Include(u => u.Followers)
				.Include(u => u.Followings).Include(u => u.BlockedUsers)
				.Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime))
				.FirstOrDefaultAsync(u => u.Id == user.Id);
			var userId = user.Id;
			VmSinglePost vmSinglePost = new VmSinglePost();
			vmSinglePost.ApplicationUser = user;
			vmSinglePost.post = ClsUserPosts.getPost(postId);
			foreach (var like in vmSinglePost.post.Likes)
			{
				if (like.UserId == userId)
				{
					vmSinglePost.IsLikedByCurrentUser = true;
					break;
				}
				else
				{
					vmSinglePost.IsLikedByCurrentUser = false;
				}

			}
			ViewBag.titels = ClsUserPosts.getTitle(vmSinglePost.post);
			ViewBag.ProfileImage = user.ProfileImage;

			ViewBag.PostImage = vmSinglePost.post.PostImage.ImageName;
			ViewBag.Notifications = user.Notifications;
			if (user.BlockedUsers.Select(u => u.Id).Contains(vmSinglePost.post.UserId) || vmSinglePost.post.User.BlockedUsers.Select(u => u.Id).Contains(user.Id))
			{
				ViewBag.block = 1;
			}
			else
			{
				ViewBag.block = 0;
			}
			return View(vmSinglePost);

		}
		public async Task<IActionResult> About()
		{
			var user = await _userManager.GetUserAsync(User);
			user = await _userManager.Users.Include(u => u.Followers)
				.Include(u => u.Followings)
				.Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime))
				.FirstOrDefaultAsync(u => u.Id == user.Id);
			if (user != null)
			{

				FillViewBags(user);
				return View(user);
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> blocked()
		{
			var user = await _userManager.GetUserAsync(User);
			user = await _userManager.Users.Include(u => u.Followers).Include(u => u.BlockedUsers)
				.Include(u => u.Followings)
				.Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime))
				.FirstOrDefaultAsync(u => u.Id == user.Id);
			if (user != null)
			{

				FillViewBags(user);
				return View(user);
			}
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Intersts()
		{
			var user = await _userManager.GetUserAsync(User);
			user = await _userManager.Users.Include(u => u.Followers)
				.Include(u => u.Followings).Include(u=>u.Interests)
				.Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime))
				.FirstOrDefaultAsync(u => u.Id == user.Id);

			if (user != null)
			{


				FillViewBags(user);
				VmInterests vmI = new VmInterests();
				vmI.AllInterests = ClsIntersts.GetAllIntersts();

				foreach (var interest in user.Interests)
				{
					vmI.SelectedInterests.Add(interest.InterestId);
				}
				vmI.AllInterests = ClsIntersts.GetAllIntersts();
				vmCompoUserInterst.ApplicationUser = user;
				vmCompoUserInterst.VmInterests = vmI;

				return View(vmCompoUserInterst);
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> Interests(VmInterests model)
		{
			var user = await _userManager.GetUserAsync(User);
			user = ClsUserPosts.GetIntersts(user.Id);
			model.AllInterests = ClsIntersts.GetAllIntersts();
			if (!ModelState.IsValid || model.SelectedInterests.Count == 0)
			{
				return Json(new { success = false, message = "Select at least one interest" });
			}

			try
			{
				if (user != null)
				{
					user.Interests.Clear();// Clear existing interests
					var result = await _userManager.UpdateAsync(user);
					user = ClsUserPosts.GetIntersts(user.Id);
					foreach (int interestId in model.SelectedInterests)
					{
						var interest = ClsIntersts.getInterest(interestId);
						user.Interests.Add(interest);
					}
					result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						return Json(new { success = true, message = "Update successful." });
					}
				}

			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Select at least one interest" });

			}
			return View("Interests", vmCompoUserInterst);
		}
		public async Task<IActionResult> UpdateBasicInfo(ApplicationUser CompoModel)
		{
			ApplicationUser model = CompoModel;
			var user = await _userManager.GetUserAsync(User);

			if (user != null)
			{
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.Email = model.Email;
				//user.Gender = model.Gender;
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return Json(new { success = true, message = user.FirstName + " " + user.LastName });
				}
				foreach (var error in result.Errors)
				{
					if (error.Code == "DuplicateEmail")
					{
						ModelState.AddModelError("Email", error.Description);
						return Json(new { success = false, message = error.Description });
					}
					if (error.Code == "InvalidEmail")
					{
						ModelState.AddModelError("Email", error.Description);
						return Json(new { success = false, message = error.Description });
					}
				}
			}
			return Json(new { success = true, message = "Update successful." });

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
		public async Task<IActionResult> AddPost(string enteredText, IFormFile uploadedImage, DateTime Date, int? groupId)
		{

			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			if (await checkText(enteredText) && await ClsUserPosts.AddPost(enteredText, userId, uploadedImage, Date, groupId))
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
		public async Task<bool> checkText(string enteredText)
		{
			PredictionHandler p = new PredictionHandler();
			if (await p.CheckSentenceForProfanityAsync(enteredText))
			{
				return false;
			}
			else
			{
				return true;
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
		public async Task<IActionResult> RemoveFollowing(string FollowingId)
		{

			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (await ClsFollow.RemoveFollowing(FollowingId, userId))
			{
				return Ok();
			}
			else
			{
				return Error();
			}
		}
		[HttpPost]
		public async Task<IActionResult> RemoveFollower(string FollowerId)
		{
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			if (await ClsFollow.RemoveFollower(FollowerId, userId))
			{
				return Ok();
			}
			else
			{
				return Error();

			}

		}

		[HttpPost]
		public async Task<IActionResult> EditProfileImage(IFormFile ProfileImage)
		{
			string ImageName = ClsHelper.UploadImage(ProfileImage, "user");
			if (!string.IsNullOrEmpty(ImageName))
			{

				var user = await _userManager.GetUserAsync(User);
				user = await _userManager.Users.Include(u => u.Followers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == user.Id);

				if (user != null)
				{
					user.ProfileImage = ImageName;
					await _userManager.UpdateAsync(user);

				}
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> EditCoverImage(IFormFile ProfileImage)
		{
			string ImageName = ClsHelper.UploadImage(ProfileImage, "user");
			if (!string.IsNullOrEmpty(ImageName))
			{

				var user = await _userManager.GetUserAsync(User);
				user = await _userManager.Users.Include(u => u.Followers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == user.Id);

				if (user != null)
				{
					user.CoverImage = ImageName;
					await _userManager.UpdateAsync(user);

				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> CreateGroup(string groupName, string groupDescription, int groupSpecification)
		{
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			var Id = ClsGroups.CreateGroup(userId, groupName, groupDescription, groupSpecification);
			if (Id != 0)
			{
				Response.Cookies.Append("Id", Id.ToString());
				return Json(new { Id2 = Id });
			}
			else
			{
				return Error();

			}

		}

		public IActionResult Privacy()
		{
			return PartialView();
		}
		[HttpPost]

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
