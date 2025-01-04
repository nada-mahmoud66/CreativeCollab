using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace CreativeCollab2.BL
{
	public interface Icomments
	{
		public bool AddComment(int postId, string text, string userId);
		public int GetCommentsCount(int postId);
	}
	public class ClsComments : Icomments
	{
		CreativeCollabContext ctx;
		private readonly INotifications _notification;

		public ClsComments(CreativeCollabContext _ctx, INotifications notification)
		{
			ctx = _ctx;
			_notification = notification;
		}
		public bool AddComment(int postId, string text, string userId)
		{

			try
			{
				Comment comment = new Comment
				{

					CommentText = text,
					CommentDate = DateTime.Now,
					PostId = postId,
					UserId = userId,
				};
				ctx.Comments.Add(comment);
				ctx.SaveChanges();
				var commentCount = ctx.Comments.Count(c => c.PostId == postId);
				var currUsr = ctx.Users.Where(u => u.Id == userId).FirstOrDefault();
				var post = ctx.Posts.Where(p => p.PostId == postId).Include(p => p.User).FirstOrDefault();
				string postOwnerId = post.User.Id;
				 _notification.Send(userId, postOwnerId, " commented on your post", "comment", postId, currUsr.ProfileImage, currUsr.FirstName + " " + currUsr.LastName,null);

				// Return the updated comment count as JSON
				return true;

			}
			catch (Exception ex)
			{
				// Handle the exception, log it, and return an error response
				return false;
			}
		}
		public int GetCommentsCount(int postId)
		{
			return ctx.Comments.Count(c => c.PostId == postId);
		}
	}
}
