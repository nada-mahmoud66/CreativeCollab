using Microsoft.AspNetCore.Mvc;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using CreativeCollab2.BL;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.Controllers
{
	public class GroupController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<GroupController> _logger;

		IGroups _Igroup;
		Iposts _Iposts;
		public GroupController(ILogger<GroupController> logger, UserManager<ApplicationUser> userManager, IGroups Igroup, Iposts iposts)
		{
			_userManager = userManager;
			_Igroup = Igroup;			_Iposts = iposts;
			_logger = logger;
		}
		public async Task<IActionResult> ClickedGroup(int groupId)
		{
			Group group = _Igroup.getGroup(groupId);
			var user = await _userManager.GetUserAsync(User);
			if (user.Id == group.CreatorUserId)
				return RedirectToAction("GroupAdmin", new { groupId = groupId });
			else
				return RedirectToAction("GroupMember", new { groupId = groupId });
		}
		public async Task<IActionResult> GroupAdmin(int groupId)
		{

			VmGroupAdmin vm = new VmGroupAdmin();
			var user = await _userManager.GetUserAsync(User);
			vm.CurrentUser = _userManager.Users.Include(u => u.Followings).Include(u => u.Followers).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefault(u => u.Id == user.Id);
			vm.group = _Igroup.getGroup(groupId);
			vm.group.Posts = vm.group.Posts.OrderByDescending(p => p.PostDateTime).ToList();
			ViewBag.titels2 = _Iposts.getTitleList(vm.group.Posts.ToList());
			vm.FriendsToInvite = _Igroup.getFriendsToInvite(user.Id, groupId);
			vm.NoOfMembers = vm.group.Users.Count();
			ViewBag.Followings = user.Followings.ToList();
			ViewBag.Notifications = user.Notifications;
			ViewBag.ProfileImage = user.ProfileImage;
			ViewBag.meID = user.Id;
			ViewBag.FirstName = user.FirstName;
			ViewBag.LastName = user.LastName;

			if (user == vm.group.CreatorUser)
				return View(vm);
			else
				return RedirectToAction("GroupMember", new { groupId = groupId });
		}
		public async Task<IActionResult> GroupMember(int groupId)
		{
			VmGroupMember vm = new VmGroupMember();
			var user = await _userManager.GetUserAsync(User);
			ViewBag.ProfileImage = user.ProfileImage;
			vm.CurrentUser = _userManager.Users.Include(u => u.Followings).Include(u => u.Followers).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefault(u => u.Id == user.Id);
			ViewBag.Followings = user.Followings.ToList();
			vm.group = _Igroup.getGroup(groupId);
			vm.group.Posts = vm.group.Posts.OrderByDescending(p => p.PostDateTime).ToList();
			foreach (var post in vm.group.Posts.ToList())
			{
				if (post.User.BlockedUsers.Select(u => u.Id).Contains(user.Id) || user.BlockedUsers.Select(u => u.Id).Contains(post.UserId))
				{
					vm.group.Posts.Remove(post);
				}
			}
			ViewBag.titels2 = _Iposts.getTitleList(vm.group.Posts.ToList());
			ViewBag.Notifications = user.Notifications;
			vm.isMember = _Igroup.isMember(user.Id, groupId);
			vm.FriendsToInvite = _Igroup.getFriendsToInvite(user.Id, groupId);
			vm.NoOfMembers = vm.group.Users.Count();
			ViewBag.meID = user.Id;
			ViewBag.FirstName = user.FirstName;
			ViewBag.LastName = user.LastName;

			if (user != vm.group.CreatorUser)
				return View(vm);
			else
				return RedirectToAction("GroupAdmin", new { groupId = groupId });
		}
		public IActionResult UnfollowGroup(string userId, int groupId)
		{
			_Igroup.unfollowGroup(userId, groupId);
			return RedirectToAction("GroupMember", new { groupId = groupId });
		}
		public IActionResult FollowGroup(string userId, int groupId)
		{
			_Igroup.followGroup(userId, groupId);
			return RedirectToAction("GroupMember", new { groupId = groupId });
		}
		public IActionResult DeleteGroup(int groupId)
		{
			_Igroup.deleteGroup(groupId);
			return Redirect("/TimeLine/Index");
		}
		[HttpPost]
		public IActionResult EditGroupCover(IFormFile coverImageFile, int groupId)
		{
			_Igroup.editGroupCover(coverImageFile, groupId);
			return RedirectToAction("GroupAdmin", new { groupId = groupId });
		}
		public async Task<IActionResult> GroupMembersForMember(int groupId)
		{
			VmGroupMembersForMember vm = new VmGroupMembersForMember();
			var user = await _userManager.GetUserAsync(User);
			ViewBag.ProfileImage = user.ProfileImage;
			vm.CurrentUser = _userManager.Users.Include(u => u.Followings).Include(u => u.Followers).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefault(u => u.Id == user.Id);
			ViewBag.Followings = user.Followings.ToList(); vm.group = _Igroup.getGroup(groupId);
			vm.isMember = _Igroup.isMember(user.Id, groupId);
			vm.FriendsToInvite = _Igroup.getFriendsToInvite(user.Id, groupId);
			vm.NoOfMembers = vm.group.Users.Count();
			vm.MutualFriends = _Igroup.getMutualFirends(user.Id, groupId);
			vm.RestOfGroupFollowers = _Igroup.getRestOfGroupFollowers(user.Id, groupId);
			vm.isFollowingAdmin = _Igroup.isFollowingAdmin(user.Id, vm.group.CreatorUser.Id);
			ViewBag.Notifications = user.Notifications;

			return View(vm);
		}
		[HttpPost]
		public IActionResult UnfollowFriend(string userId, string friendId)
		{
			_Igroup.unfollowFriend(userId, friendId);
			return Ok();
		}
		[HttpPost]
		public IActionResult FollowMember(string userId, string memberId)
		{
			_Igroup.followMember(userId, memberId);
			return Ok();
		}
		public async Task<IActionResult> GroupMembersForAdmin(int groupId)
		{
			VmGroupMembersForAdmin vm = new VmGroupMembersForAdmin();
			var user = await _userManager.GetUserAsync(User);
			ViewBag.ProfileImage = user.ProfileImage;
			vm.CurrentUser = _userManager.Users.Include(u => u.Followings).Include(u => u.Followers).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefault(u => u.Id == user.Id);
			ViewBag.Followings = user.Followings.ToList(); vm.group = _Igroup.getGroup(groupId);
			vm.FriendsToInvite = _Igroup.getFriendsToInvite(user.Id, groupId);
			vm.NoOfMembers = vm.group.Users.Count();
			vm.MutualFriends = _Igroup.getMutualFirends(user.Id, groupId);
			vm.RestOfGroupFollowers = _Igroup.getRestOfGroupFollowers(user.Id, groupId);
			ViewBag.Notifications = user.Notifications;

			return View(vm);
		}
		[HttpPost]
		public IActionResult DeleteMemberFromGroup(string memberId, int groupId)
		{
			_Igroup.unfollowGroup(memberId, groupId);
			return Ok();
		}
		[HttpPost]
		public IActionResult EditPost(int postId, string NewText)
		{
			_Iposts.UpdatePost(NewText, postId);
			return Ok();
		}
		[HttpPost]
		public IActionResult DeletePost(int postId)
		{
			_Iposts.RemovePost(postId);
			return Ok();
		}
		[HttpPost]
		public async Task<IActionResult> AddPost(IFormFile uploadedImage, string enteredText, DateTime Date, int groupId)
		{
			var user = await _userManager.GetUserAsync(User);
			await _Iposts.AddPost(enteredText, user.Id, uploadedImage, Date, groupId);
			return Ok();
		}
		[HttpPost]
		public IActionResult InviteFriend(string userId, string friendId, int groupId)
		{
			_Igroup.friendToInvite(userId, friendId, groupId);
			return Ok();
		}
	}
}
