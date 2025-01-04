using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace CreativeCollab2.BL
{
    public interface Iposts
    {
        public int GetLastPostId();
        public bool UpdatePost(string NewText, int postId);
        public Task<bool> AddPost(string enteredText, string userId, IFormFile uploadedImage, DateTime Date, int? groupId);
        public bool RemovePost(int postId);
        //public Task<bool> AddPostInGroup(string enteredText, string userId, IFormFile uploadedImage, DateTime Date, int? groupId);
        public Task<List<Post>> GetProfileUserPostsByID(string userId);
        public Task<List<Post>> GetTimeLinePostsByID(string userId);
        public List<Post> getNewUserDiscovery(string userId);
        public ApplicationUser GetIntersts(string userId);
        public List<Post> recPosts(List<int> ids, string userId);
        public List<Post> SearchByImage(List<int> ids, string userId);
        public bool CheckLikes(string userId);

        public List<string> getTitleList(List<Post> titels);
        public string getTitle(Post post);

        public Post getPost(int postId);

        public List<Post> getPostsFromImagePath(List<string> pathes);

    }

    public class ClsUserPosts : Iposts
    {
        CreativeCollabContext ctx;
        Iintersts ClsInterest;
        private readonly UserManager<ApplicationUser> _userManager;
        //Ihelper ClsHelper;
        PredictionHandler p = new PredictionHandler();
		private readonly IHubContext<TimelineHub> _hubContext;
		public ClsUserPosts(Iintersts i, CreativeCollabContext _ctx, UserManager<ApplicationUser> userManager, IHubContext<TimelineHub> hubContext)
        {
            _userManager = userManager;
            ctx = _ctx;
            ClsInterest = i;
            _hubContext = hubContext;
		}
        static List<T> Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                // Swap list[i] and list[j]
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
            return list;
        }
        public List<Post> recPosts(List<int> ids, string userId)
        {

            List<Post> temp = new List<Post>();
            List<Post> tempNotMe = new List<Post>();

            var num = ids.Count;
            var me = _userManager.Users.Include(u => u.Interests).FirstOrDefault(u => u.Id == userId);

            foreach (int id in ids)
            {
                var post = ctx.Posts.Include(p => p.User)
                                       .Include(p => p.PostImage).ThenInclude(i => i.Interest)
                                       .Include(p => p.Comments).ThenInclude(i => i.User)
                                       .Include(p => p.Likes)
                                       .Where(p => (p.PostId == id) && (p.UserId != userId))
                                       .FirstOrDefault();
                if (post != null)
                {
                    temp.Add(post);
                    //	if (!(me.Interests.Select(i => i.InterestId).ToList().Contains((int)post.PostImage.InterestId)))
                    //	{
                    //		tempNotMe.Add(post);
                    //	}
                    //	else
                    //	{
                    //		temp.Add(post);
                    //	}
                    //}
                }
                //temp.AddRange(tempNotMe.Take((int)0.5 * tempNotMe.Count));

            }
            //var userIntId = me.Interests.Select(i => i.InterestId).ToList();
            //var posts = ctx.Posts.Include(p => p.PostImage).Include(p => p.Likes).Include(p => p.User).Include(p => p.Comments).ThenInclude(p => p.User).ToList();
            ////var posts = ctx.Posts.Include(p=>p.PostImage).Where(userIntId.Contains( (int)p=>p.PostImage.InterestId)).ToList();

            //foreach (var post in posts)
            //{
            //	if ((!(post.Likes.Select(u => u.UserId).ToList().Contains(userId))) && (userIntId.Contains((int)post.PostImage.InterestId)))
            //	{
            //		temp.Add(post);
            //	}
            //}

            return Shuffle(temp);
        }
        public bool CheckLikes(string userId)
        {
            if (ctx.Likes.Select(u => u.UserId).ToList().Contains(userId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Post> getNewUserDiscovery(string userId)
        {
            List<Post> temp = new List<Post>();
            var me = _userManager.Users.Include(u => u.Interests).FirstOrDefault(u => u.Id == userId);

            var userIntId = me.Interests.Select(i => i.InterestId).ToList();
            var posts = ctx.Posts.Include(p => p.User).ThenInclude(p => p.BlockedUsers).Include(p => p.PostImage).Include(p => p.Likes).Include(p => p.Comments).ThenInclude(p => p.User).ToList();

            foreach (var post in posts)
            {

                if ((!(post.Likes.Select(u => u.UserId).ToList().Contains(userId))) && (userIntId.Contains((int)post.PostImage.InterestId)))
                {
                    temp.Add(post);
                }
            }

            return Shuffle(temp.Take(20).ToList());
        }
        public List<string> getTitleList(List<Post> posts)
        {
            List<string> titles = new List<string>();
            foreach (var post in posts)
            {
                var ImageName = post.PostImage.ImageName;
                var intListID = ctx.Images.Where(i => i.ImageName == ImageName).Select(i => i.InterestId).ToList();
                List<string> intName = new List<string>();
                foreach (int v in intListID)
                {
                    intName.Add(ClsInterest.getInterest(v).InterestName);
                }
                titles.Add(string.Join(" ,", intName));

            }
            return titles;

        }
        public string getTitle(Post post)
        {
            List<string> titles = new List<string>();


            var ImageName = post.PostImage.ImageName;
            var intListID = ctx.Images.Where(i => i.ImageName == ImageName).Select(i => i.InterestId).ToList();
            List<string> intName = new List<string>();
            foreach (int v in intListID)
            {
                intName.Add(ClsInterest.getInterest(v).InterestName);
            }
            titles.Add(string.Join(" ,", intName));

            return titles[0];
        }
        public Post getPost(int postId)
        {
            return ctx.Posts.Include(u => u.User).ThenInclude(u => u.BlockedUsers)
                   .Include(p => p.Comments)
                   .ThenInclude(p => p.User)
                   .ThenInclude(p => p.Likes)
                   .Include(p => p.Likes)
                   .Include(p => p.PostImage)
                   .ThenInclude(p => p.Interest).Include(p => p.Group).FirstOrDefault(a => a.PostId == postId);
        }

        public async Task<List<Post>> GetProfileUserPostsByID(string userId)
        {
            var posts = await ctx.Posts.Include(p => p.PostImage)
                   .Include(p => p.Comments)
                   .ThenInclude(p => p.User)
                   .Include(p => p.Likes)
                   .Where(p => (p.IsDeleted == false) && (p.GroupId == null) && (p.UserId == userId)).OrderByDescending(p => p.PostDateTime).ToListAsync();
            //foreach (var post in posts)
            //{
            //	var query = $@"SELECT

            //                 STRING_AGG(int.InterestName, ' ,') AS InterestNames
            //             FROM
            //                 Images i
            //             JOIN
            //                 Interests int ON i.interestId = int.interestId
            //             WHERE
            //                 i.ImageId = {post.PostImageId}
            //             GROUP BY
            //                 ImageName;";

            //	//post.Titles = ctx.Images.FromSqlRaw(query).ToString();

            //}
            return posts;
        }


        public bool RemovePost(int postId)
        {
            try
            {

                var post = ctx.Posts.FirstOrDefault(p => p.PostId == postId);
                if (post != null)
                {
                    post.IsDeleted = true;
                    ctx.SaveChanges();

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public async Task<bool> AddPost(string enteredText, string userId, IFormFile uploadedImage, DateTime Date, int? groupId)
        {
            //var post = ctx.Posts.Where(p => p.PostId == 515).Include(p => p.PostImage).FirstOrDefault();
            //var imageInPost = post.PostImage.ImageName;
            //var intersts = ctx.Images.Include(i => i.Interest).Where(i => i.ImageName == imageInPost).Select(i => i.Interest.InterestName).ToList();

            List<Interest> AllInterest = ClsInterest.GetAllIntersts();

            try
            {

                string imageName = ClsHelper.UploadImage(uploadedImage, "post");
                var imagePath = "D:\\ASP.NET\\CreativeCollabOla\\CreativeCollab2\\CreativeCollab2\\wwwroot\\images\\post\\" + imageName;

                var predictions = await p.GetPredictionsForImageAsync(imagePath);
                var jsonObject = JObject.Parse(predictions);
                string[] predictions1 = jsonObject["predictions"].ToObject<string[]>();

                Image image = new Image();
                foreach (var u in predictions1)
                {
                    image = new Image
                    {
                        ImageName = imageName,
                        InterestId = AllInterest.Where(i => i.InterestName == u).Select(i => i.InterestId).FirstOrDefault()
                    };
                    ctx.Images.Add(image);
                }

                ctx.SaveChanges();

				var post = new Post
				{
					PostImageId = image.ImageId,
					PostText = enteredText,
					UserId = userId,
					PostDateTime = Date,
					GroupId = groupId,
				};
				ctx.Posts.Add(post);
				ctx.SaveChanges();
				var user = ctx.Users.Include(u => u.Followers).Where(U => U.Id == userId).FirstOrDefault();
				var group = ctx.Groups.Include(g => g.Users).Where(g => g.GroupId == groupId).FirstOrDefault();
				var followersIds = user.Followers.Select(f => f.Id).ToList();
				if (groupId != null)
				{
					var groupFollowers = group.Users.Select(u => u.Id).ToList();
					followersIds = followersIds.Union(followersIds).Distinct().ToList();
					await _hubContext.Clients.All.SendAsync("ReceivePostInGroup", userId, post.PostId, user.ProfileImage, user.FirstName + " " + user.LastName, Date, enteredText, getTitle(post), image.ImageName, groupId, group.GroupName);
					await _hubContext.Clients.Users(followersIds).SendAsync("ReceivePostInTimelne", userId, post.PostId, user.ProfileImage, user.FirstName + " " + user.LastName, Date, enteredText, getTitle(post), image.ImageName, groupId, group.GroupName);

				}
				else
				{
					await _hubContext.Clients.Users(followersIds).SendAsync("ReceivePostInTimelne", userId, post.PostId, user.ProfileImage, user.FirstName + " " + user.LastName, Date, enteredText, getTitle(post), image.ImageName, null, null);
				}
				return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool UpdatePost(string NewText, int postId)
        {

            try
            {

                var post = ctx.Posts.FirstOrDefault(p => p.PostId == postId);
                post.PostText = NewText;
                ctx.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public int GetLastPostId()
        {
            var lastId = ctx.Posts
                     .OrderByDescending(e => e.PostId)
                     .Select(e => e.PostId)
                      .FirstOrDefault();

            return lastId;
        }

        public ApplicationUser GetIntersts(string userId)
        {

            return ctx.Users.Include(u => u.Interests).Where(U => U.Id == userId).FirstOrDefault();


        }

        public async Task<List<Post>> GetTimeLinePostsByID(string userId)
        {
            var user = await _userManager.Users.Include(u => u.Groups).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == userId);
            var followingsIds = user.Followings.Select(f => f.Id).ToList();
            var GroupsIds = user.Groups.Select(g => g.GroupId).ToList();
            var GroupsNames = user.Groups.Select(g => g.GroupName).ToList();
            var groupPosts = new List<Post>();
            groupPosts = await ctx.Posts.Include(p => p.User)
                                           .Include(p => p.PostImage).ThenInclude(p => p.Interest)
                                           .Include(p => p.Comments).ThenInclude(i => i.User)
                                           .Include(p => p.Likes)
                                           .Where(p => ((p.GroupId != null) && (p.IsDeleted == false) && (GroupsIds.Contains((int)p.GroupId))))
                                           .ToListAsync();
            var userPosts = new List<Post>();
            userPosts = await ctx.Posts.Include(p => p.User)
                                       .Include(p => p.PostImage).ThenInclude(p => p.Interest)
                                       .Include(p => p.Comments).ThenInclude(i => i.User)
                                       .Include(p => p.Likes)
                                       .Where(p => (p.GroupId == null) && (p.IsDeleted == false) && (followingsIds.Contains(p.UserId)))
                                       .ToListAsync();

            var timelinePosts = new List<Post>();
            if (userPosts.Count == 0 && groupPosts.Count == 0)
            {
                return new List<Post>();
            }
            else if (userPosts.Count == 0)
            {
                return groupPosts.OrderByDescending(p => p.PostDateTime).ToList();
            }
            else if (groupPosts.Count == 0)
            {
                return userPosts.OrderByDescending(p => p.PostDateTime).ToList();
            }
            timelinePosts = groupPosts.Concat(userPosts).OrderByDescending(p => p.PostDateTime).ToList();

            return timelinePosts;
        }


        public List<Post> SearchByImage(List<int> ids, string userId)
        {
            List<Post> FinalPosts = new List<Post>();
            foreach (var id in ids)
            {
                var posts = ctx.Posts.Include(p => p.User).ThenInclude(u => u.BlockedUsers)
                                         .Include(p => p.PostImage).ThenInclude(i => i.Interest)
                                         .Include(p => p.Comments).ThenInclude(c => c.User)
                                         .Include(p => p.Likes)
                                         .Where(i => ((int)i.PostImage.InterestId == id) && (i.UserId != userId))
                                         .ToList();
                FinalPosts.AddRange(posts);

            }
            return FinalPosts;
        }


        public List<Post> getPostsFromImagePath(List<string> pathes)
        {
            List<Post> posts = new List<Post>();
            foreach (var path in pathes)
            {
                string fileName = Path.GetFileName(path);
                var post = ctx.Posts.Include(p => p.Comments).ThenInclude(p => p.User).Include(p => p.Likes).Include(p => p.User).ThenInclude(p => p.BlockedUsers).Include(p => p.PostImage).Where(p => p.PostImage.ImageName == fileName).FirstOrDefault();
                if (post != null)
                {
                    posts.Add(post);
                }
            }
            return posts;
        }

    }
}
