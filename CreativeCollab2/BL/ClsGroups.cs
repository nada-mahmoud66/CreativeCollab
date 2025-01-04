using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.BL
{
	public interface Igroups
	{
		public List<Group> GetAllGroupsByUserId(string userId);
		public List<Group> GetOwnGroups(string userId);
		//public Group getGroupById(int groupId , string UserId);
		public int CreateGroup(string userId, string GroupName, string GroupDescription, int InterestId);
	}
	public class ClsGroups : Igroups
	{
		CreativeCollabContext ctx;
		UserManager<ApplicationUser> _userManager;
		public ClsGroups(CreativeCollabContext _ctx, UserManager<ApplicationUser> userManager)
		{
			ctx = _ctx;
			_userManager = userManager;
		}

		public List<Group> GetAllGroupsByUserId(string userId)
		{
			List<Group> groups = new List<Group>();

			var user = _userManager.Users.Include(u => u.GroupsNavigation).FirstOrDefault(u => u.Id == userId);
			groups = user.GroupsNavigation.ToList();
			return groups;

		}
		//public Group getGroupById(string UserId)
		//{
		//	var groups = ctx.Groups.Where(h => h.CreatorUserId == UserId).OrderByDescending(u => u.CreatedDateTime).FirstOrDefault();
		//	return group;
		//}

		public List<Group> GetOwnGroups(string userId)
		{
			List<Group> groups = new List<Group>();
			groups = ctx.Groups.Where(g => g.CreatorUserId == userId).ToList();
			return groups.ToList();
		}

		public int CreateGroup(string userId, string GroupName, string GroupDescription, int InterestId)
		{
			try
			{
				var user = ctx.Users.FirstOrDefault(u => u.Id == userId);
				Group group = new Group()
				{
					GroupName = GroupName,
					Description = GroupDescription,
					CreatedDateTime = DateTime.Now,
					CreatorUserId = userId,
					InterestId = InterestId,
					CoverImage = "default_image.png",
					IsDeleted = false
				};

				ctx.Groups.Add(group);
				ctx.SaveChanges();
				return group.GroupId;
			}
			catch (Exception e)
			{
				return 0;
			}
		}

	}
}
