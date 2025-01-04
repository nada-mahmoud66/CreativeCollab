using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.BL
{
	public interface Iserch
	{

		public serchResult getSearchResult(string subStr, string userId);
	}


	public class ClsSearch : Iserch
	{
		CreativeCollabContext ctx;
		IChats ClsChats;
		UserManager<ApplicationUser> _userManager;
		public ClsSearch(IChats c, CreativeCollabContext _ctx, UserManager<ApplicationUser> userManager)
		{
			ctx = _ctx;
			_userManager = userManager;
			ClsChats = c;
		}

		public List<ApplicationUser> getUsersSearch(string subStr, string userId)
		{
			List<ApplicationUser> results = new List<ApplicationUser>();
			try
			{
				var user = ctx.Users.Include(u => u.BlockedUsers).Include(u => u.BlockingUsers).FirstOrDefault(u => u.Id == userId);
				results = ctx.Users.Include(u => u.BlockingUsers).Where(u => u.FirstName.StartsWith(subStr)).ToList();
				foreach (var res in results)
				{
					if (user.BlockedUsers.Select(u => u.Id).Contains(res.Id) || res.BlockedUsers.Select(u => u.Id).Contains(userId))
					{
						results.Remove(res);
					}
				}
				return results;
			}
			catch (Exception ex)
			{

			}
			return results;
		}
		public List<Group> getGroupsSearch(string subStr)
		{
			List<Group> results = new List<Group>();
			try
			{
				results = ctx.Groups.Where(u => u.GroupName.StartsWith(subStr)).ToList();
				return results;
			}
			catch (Exception ex)
			{

			}
			return results;
		}
		public List<Post> getPostsSearch(string subStr)
		{
			List<Post> results = new List<Post>();
			try
			{
				results = ctx.Posts.Include(p => p.User).ThenInclude(p => p.BlockedUsers).
					Include(p => p.Likes).
					Include(p => p.PostImage).
					Include(p => p.Group).
					Include(p => p.Comments).ThenInclude(p => p.User)
					.Where(u => u.PostText.Contains(subStr)).ToList();
				return results;
			}
			catch (Exception ex)
			{

			}
			return results;
		}
		public serchResult getSearchResult(string subStr, string userId)
		{
			List<ApplicationUser> UsersResults = new List<ApplicationUser>();
			List<Group> GroupResults = new List<Group>();
			List<Post> PostRes = new List<Post>();
			serchResult result = new serchResult();


			try
			{
				UsersResults = getUsersSearch(subStr, userId).ToList();
				GroupResults = getGroupsSearch(subStr).ToList();
				PostRes = getPostsSearch(subStr);
				foreach (var post in PostRes.ToList())
				{
					if (ClsChats.cheakIBlocked(userId, post.UserId) || ClsChats.cheakBlock(userId, post.UserId))
					{
						PostRes.Remove(post);
					}
				}
				result.users = UsersResults;
				result.groups = GroupResults;
				result.posts = PostRes;
				result.sub = subStr;
				return result;


			}
			catch (Exception ex)
			{

			}
			return result;
		}
	}
}
