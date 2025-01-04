using CreativeCollab2.Models;
using Microsoft.EntityFrameworkCore;
using static CreativeCollab2.BL.ClsNotifications;

namespace CreativeCollab2.BL
{
	public interface Ilikes
	{
		public Task<int> GetTotalLikesPerWeek(string userId);
		public Task<bool> AddLike(int postId, string userId);
		public Task<bool> RemoveLike(int postId, string userId);

	}
	public class ClsLikes : Ilikes
	{
		CreativeCollabContext ctx;
		private readonly INotifications _notification;
		public ClsLikes(CreativeCollabContext _ctx, INotifications notification)
		{
			ctx = _ctx;
			_notification = notification;
		}
		public async Task<bool> AddLike(int postId, string userId)
		{
			try
			{
				ctx.Likes.Add(new Like
				{
					PostId = postId,
					UserId = userId,
				});
				
				ctx.SaveChanges();
				var currUsr = ctx.Users.Where(u => u.Id == userId).FirstOrDefault();
				var post = ctx.Posts.Where(p => p.PostId == postId).Include(p => p.User).FirstOrDefault();
				string postOwnerId = post.User.Id;
				await _notification.Send(userId, postOwnerId, " liked your post", "like", postId,currUsr.ProfileImage, currUsr.FirstName + " " + currUsr.LastName,null);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<bool> RemoveLike(int postId, string userId)
		{

			try
			{
				var like = ctx.Likes.Where(l => (l.UserId == userId) && (l.PostId == postId)).FirstOrDefault();
				ctx.Likes.Remove(like);
				ctx.SaveChanges();
				await _notification.Unsend(postId, userId, "like");
				return true;
			}
			catch (Exception e)
			{
				return false;
			}

		}

		public async Task<int> GetTotalLikesPerWeek(string userId)
		{
			DateTime lastWeek = DateTime.Now.AddDays(-7);
			var myPosts = ctx.Posts.Where(l => (l.UserId == userId)).ToList();
			//var likesCount = myPosts.Where(p => p.Likes).Sum(l => l.Likes.Count);
			//return likesCount;
			return 0;
		}
	}
}