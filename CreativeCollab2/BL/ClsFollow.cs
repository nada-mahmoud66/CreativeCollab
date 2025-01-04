using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.BL
{
	public interface Ifollow
	{
		//public Task<List<ApplicationUser>> GetFollowingListbyId(string userId);
		//public Task<List<ApplicationUser>> GetFollowersListbyId(string userId);
		public Task<bool> RemoveFollower(string FollowerId, string UserId);
		public Task<bool> RemoveFollowing(string FollowingId, string UserId);
		public Task<bool> BlockUser(string meId, string UserId);
		public Task<bool> UnBlockUser(string meId, string UserId);

		public Task<bool> AddFollowing(string FollowingId, string UserId);




	}
	public class ClsFollow : Ifollow
	{
		CreativeCollabContext ctx;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly INotifications _notification;
		public ClsFollow(CreativeCollabContext _ctx, UserManager<ApplicationUser> userManager, INotifications notification)
		{
			_userManager = userManager;
			_notification = notification;
		}

		//public async Task<List<ApplicationUser>> GetFollowersListbyId(string userId)
		//{
		//	var followingUsers = await ctx.Users
		//		.FromSqlRaw("SELECT u.* FROM AspNetUsers u " +
		//					"INNER JOIN Followers f ON u.Id = f.FollowerID " +
		//					"WHERE f.FollowingID = {0}", userId)
		//		.ToListAsync();
		//	return followingUsers;
		//}
		//public async Task<List<ApplicationUser>> GetFollowingListbyId(string userId)
		//{
		//	var followingUsers = await ctx.Users
		//		.FromSqlRaw("SELECT u.* FROM AspNetUsers u " +
		//					"INNER JOIN Followers f ON u.Id = f.FollowingID " +
		//					"WHERE f.FollowerID = {0}", userId)
		//		.ToListAsync();

		//	return followingUsers;
		//}
		public async Task<bool> BlockUser(string meId, string UserId)
		{
			try
			{
				var me = await _userManager.Users.Include(u => u.BlockedUsers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == meId);
				var him = await _userManager.Users.Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == UserId);
				me.Followings.Remove(him);
				him.Followings.Remove(me);

				me.BlockedUsers.Add(him);

				var result = await _userManager.UpdateAsync(me);
				var result2 = await _userManager.UpdateAsync(him);

				await _notification.Unsend(null, UserId, "follow");
				if (result.Succeeded && result2.Succeeded)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public async Task<bool> UnBlockUser(string meId, string UserId)
		{
			try
			{
				var me = await _userManager.Users.Include(u => u.BlockedUsers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == meId);
				var him = await _userManager.Users.Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == UserId);


				me.BlockedUsers.Remove(him);

				var result = await _userManager.UpdateAsync(me);

				await _notification.Unsend(null, UserId, "follow");
				if (result.Succeeded)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		public async Task<bool> RemoveFollowing(string FollowingId, string UserId)
		{
			try
			{
				var user = await _userManager.Users.Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == UserId);
				var following = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == FollowingId);
				user.Followings.Remove(following);
				var result = await _userManager.UpdateAsync(user);
				await _notification.Unsend(null, UserId, "follow");
				if (result.Succeeded)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> AddFollowing(string FollowingId, string UserId)
		{
			try
			{
				var user = await _userManager.Users.Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == UserId);
				var following = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == FollowingId);
				user.Followings.Add(following);
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					await _notification.Send(UserId, FollowingId, " started follwoing you", "follow", null, user.ProfileImage, user.FirstName + " " + user.LastName, null);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> RemoveFollower(string FollowerId, string UserId)
		{
			try
			{
				var user = await _userManager.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == UserId);
				var follower = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == FollowerId);
				user.Followers.Remove(follower);
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

	}
}
