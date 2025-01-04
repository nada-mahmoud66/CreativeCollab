using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using CreativeCollab2.Models;

namespace CreativeCollab2.BL
{
	public interface IGroups
	{
		public Group getGroup(int groupId);
		public bool isMember(string userId, int groupid);
		public List<ApplicationUser> getFriendsToInvite(string userId, int groupId);
		public void unfollowGroup(string userId, int groupId);
		public void followGroup(string userId, int groupId);
		public void deleteGroup(int groupId);
		public void editGroupCover(IFormFile coverImageFile, int groupId);
		public List<ApplicationUser> getMutualFirends(string userId, int groupId);
		public List<ApplicationUser> getRestOfGroupFollowers(string userId, int groupId);
		public void unfollowFriend(string userId, string friendId);
		public void followMember(string userId, string memberId);
		public bool isFollowingAdmin(string userId, string adminId);
		public void friendToInvite(string userId, string friendId, int groupId);
	}
	public class Groups : IGroups
	{
		CreativeCollabContext _context;
		private readonly INotifications _notification;

		public Groups(CreativeCollabContext ctx, INotifications notification)
		{
			_context = ctx;
			_notification = notification;
		}
		public Group getGroup(int groupId)
		{
			return _context.Groups
				.Include(G => G.CreatorUser).Include(G => G.Users)
				.Include(G => G.Posts).ThenInclude(p => p.User).ThenInclude(u => u.BlockedUsers)
				.Include(G => G.Posts).ThenInclude(p => p.PostImage)
				.Include(G => G.Posts).ThenInclude(p => p.Likes)
				.Include(G => G.Posts).ThenInclude(p => p.Comments)
				.Where(G => G.GroupId == groupId).FirstOrDefault();
		}
		public bool isMember(string userId, int groupid)
		{
			var g = _context.Users.FromSqlRaw("SELECT u.* FROM AspNetUsers u " +
											 "JOIN UserGroupMembership ugm ON u.Id = ugm.UserID " +
											 "JOIN Groups g ON ugm.GroupID = g.GroupID " +
											 "WHERE u.Id = {0} " +
											 "AND g.GroupId = {1}", userId, groupid).FirstOrDefault();
			if (g == null)
				return false;
			else
				return true;
		}
		public List<ApplicationUser> getFriendsToInvite(string userId, int groupId)
		{
			return _context.Users.FromSqlRaw("SELECT DISTINCT u.* FROM AspNetUsers u " +
											 "JOIN Followers f1 ON u.Id = f1.FollowingID " +
											 "JOIN Followers f2 ON u.Id = f2.FollowerID " +
											 "LEFT JOIN UserGroupMembership gm ON u.Id = gm.UserID AND gm.GroupID = {0} " +
											 "WHERE f1.FollowerID = {1} " +
											 "AND f2.FollowingID = {1} " +
											 "AND gm.UserID IS NULL; ", groupId, userId).ToList();
		}
		public void unfollowGroup(string userId, int groupId)
		{
			_context.Database.ExecuteSqlRaw("DELETE FROM UserGroupMembership WHERE UserID = {0} AND GroupID = {1}", userId, groupId);
		}
		public void followGroup(string userId, int groupId)
		{
			_context.Database.ExecuteSqlRaw("INSERT INTO UserGroupMembership (UserID, GroupID) VALUES ({0}, {1})", userId, groupId);
		}
		public void deleteGroup(int groupId)
		{
			_context.Database.ExecuteSqlRaw("UPDATE Groups SET isDeleted = 1 WHERE GroupID ={0}", groupId);
		}
		public void editGroupCover(IFormFile coverImageFile, int groupId)
		{
			if (coverImageFile != null && coverImageFile.Length > 0)
			{
				// Define the directory to save the photo
				var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "groups");

				// Define the file path
				var filePath = Path.Combine(uploadsDirectory, coverImageFile.FileName);

				// Copy the contents of the uploaded photo to a FileStream
				var stream = new FileStream(filePath, FileMode.Create);
				coverImageFile.CopyTo(stream);
				_context.Database.ExecuteSqlRaw("UPDATE Groups SET CoverImage = {0} WHERE GroupID ={1}", coverImageFile.FileName, groupId);
			}

		}
		public List<ApplicationUser> getMutualFirends(string userId, int groupId)
		{
			/*return  _context.Users.FromSqlRaw("SELECT DISTINCT u.* FROM AspNetUsers u " +
											 "JOIN Followers f ON u.Id = f.FollowingID " +
											 "JOIN UserGroupMembership gm ON u.Id = gm.UserID AND gm.GroupID = {0} " +
											 "WHERE f.FollowerID = {1} " +
											 "AND gm.UserID IS NOT NULL; ", groupId, userId).ToList();*/

			/*return _context.Users.FromSqlRaw("SELECT DISTINCT u.* FROM AspNetUsers u " +
											 "JOIN Followers f ON u.Id = f.FollowingID " +
											 "JOIN UserGroupMembership gm ON u.Id = gm.UserID AND gm.GroupID = {0} " +
											 "LEFT JOIN Notifications n ON u.Id = n.UserID AND n.NotificationType = 'invitation' AND n.GroupId = {0} " +
											 "WHERE f.FollowerID = {1} " +
											 "AND gm.UserID IS NOT NULL "+
											 "AND n.NotificationID IS NULL; ", groupId, userId).ToList();*/


			return _context.Users.FromSqlRaw("SELECT DISTINCT u.* FROM AspNetUsers u " +
											 "JOIN Followers f ON u.Id = f.FollowingID " +
											 "JOIN UserGroupMembership gm ON u.Id = gm.UserID AND gm.GroupID = {0} " +
											 "WHERE f.FollowerID = {1} " +
											 "AND gm.UserID IS NOT NULL " +
											 "AND NOT EXISTS ( " +
											 "SELECT 1 FROM Notifications n " +
											 "WHERE n.UserID = u.Id " +
											 "AND n.NotificationType = 'invitation' " +
											 "AND n.GroupId = {0}); ", groupId, userId).ToList();

		}
		public List<ApplicationUser> getRestOfGroupFollowers(string userId, int groupId)
		{
			return _context.Users.FromSqlRaw("SELECT * from AspNetUsers u " +
											 "JOIN UserGroupMembership ugm ON u.Id = ugm.UserId " +
											 "where ugm.GroupID ={0} " +
											 "AND u.Id NOT IN ( " +
											 "SELECT followingId " +
											 "FROM Followers " +
											 "WHERE followerId = {1})", groupId, userId).ToList();
		}
		public void unfollowFriend(string userId, string friendId)
		{
			_context.Database.ExecuteSqlRaw("DELETE FROM Followers WHERE FollowerID = {0} AND FollowingID = {1}", userId, friendId);
		}
		public void followMember(string userId, string memberId)
		{
			_context.Database.ExecuteSqlRaw("INSERT INTO Followers (FollowerID, FollowingID) VALUES ({0}, {1})", userId, memberId);
		}
		//STOPPED HEREEEEE
		public bool isFollowingAdmin(string userId, string adminId)
		{
			var usr = _context.Users.FromSqlRaw("SELECT * from AspNetUsers u " +
												"JOIN Followers f ON u.Id = f.FollowerID " +
												"WHERE f.FollowerID = {0} " +
												"AND f.FollowingID = {1}", userId, adminId).FirstOrDefault();

			//var usr = _context.Database.ExecuteSqlRaw("SELECT * FROM Followers WHERE FollowerID = {0} AND FollowingID = {1}", userId, adminId);
			if (usr == null)
				return false;
			else
				return true;
		}

		public void friendToInvite(string userId, string friendId, int groupId)
		{
			var currUsr = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
			var group = _context.Groups.Where(p => p.GroupId == groupId).FirstOrDefault();
			_notification.Send(userId, friendId, " invited you to join " + group.GroupName + " group", "invitation", null, currUsr.ProfileImage, currUsr.FirstName + " " + currUsr.LastName, group.GroupId);
		}
	}
}
