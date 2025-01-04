using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CreativeCollab2.Controllers
{
    public class SearchController : Controller
    {
        private PredictionHandler _predictionHandler = new PredictionHandler();
        private readonly ILogger<SearchController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        Iposts ClsUserPosts;
        Iintersts ClsIntersts;
        Ilikes ClsLikes;
        Icomments ClsComments;
        Ifollow ClsFollow;
        Iserch ClsSearch;


        public SearchController(Iserch s, Ifollow i, Icomments c, Ilikes l, Iintersts interst, Iposts post, ILogger<SearchController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            ClsUserPosts = post;
            ClsIntersts = interst;
            ClsFollow = i;
            ClsLikes = l;
            ClsSearch = s;

        }

        public async void FillViewBags(ApplicationUser user)
        {
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

        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile Image, string searchText)
        {
            if (Image == null)
            {
                return RedirectToAction("SearchText", "Search", new { searchTerm = searchText });
            }

            var user = await _userManager.GetUserAsync(User);
            user = await _userManager.Users.Include(u => u.BlockedUsers).Include(u => u.Followers).Include(u => u.Followings).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefaultAsync(u => u.Id == user.Id);
            FillViewBags(user);
            List<Interest> AllInterest = ClsIntersts.GetAllIntersts();

            string imageName = ClsHelper.UploadImage(Image, "search");
            var imagePath = "D:/ASP.NET/CreativeCollabOla/CreativeCollab2/CreativeCollab2/wwwroot/images/search/" + imageName;

            var predictions = await _predictionHandler.GetPredictionsForImageAsync(imagePath);
            var jsonObject = JObject.Parse(predictions);
            string[] predictionsList = jsonObject["predictions"].ToObject<string[]>();
            int InterestId = 0;
            List<int> ids = new List<int>();
            foreach (var u in predictionsList)
            {
                InterestId = AllInterest.Where(i => i.InterestName == u).Select(i => i.InterestId).FirstOrDefault();
                ids.Add(InterestId);
            }
            var model1 = ClsUserPosts.SearchByImage(ids, user.Id);
            foreach (var post in model1.ToList())
            {
                if (user.BlockedUsers.Select(u => u.Id).Contains(post.UserId) || post.User.BlockedUsers.Select(u => u.Id).Contains(user.Id))
                {
                    model1.Remove(post);
                }
            }


            vmTimeLine vm = new vmTimeLine();


            var pathes = await _predictionHandler.GetSimilarImagesForImageAsync(imagePath);

            JObject jsonObject2;
            try
            {
                jsonObject2 = JObject.Parse(pathes);
            }
            catch (JsonReaderException ex)
            {

                ModelState.AddModelError(string.Empty, "Invalid JSON response");
                return View(new List<Post>()); // Return an empty list of posts or handle it as needed
            }

            JToken similarImagesToken = jsonObject2["similar_images"];
            if (similarImagesToken == null)
            {

                ModelState.AddModelError(string.Empty, "No similar images found");
                return View(new List<Post>());
            }

            string[] pathesList = similarImagesToken.ToObject<string[]>();

            var model2 = ClsUserPosts.getPostsFromImagePath(pathesList.ToList());
            foreach (var post in model2.ToList())
            {
                if (model1.Select(p => p.PostImageId).Contains(post.PostImageId))
                {
                    model1.Remove(post);

                }
                if (user.BlockedUsers.Select(u => u.Id).Contains(post.UserId) || post.User.BlockedUsers.Select(u => u.Id).Contains(user.Id))
                {
                    model2.Remove(post);
                }
            }
            vm.MyPosts = model1;
            vm.RecommendationPosts = model2;
            return View(vm);
        }


        private IActionResult RedirectToAction(Func<string, Task<IActionResult>> searchText, string v)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Index2(string searchTerm)
        {
            var user = await _userManager.GetUserAsync(User);

            var results = ClsSearch.getSearchResult(searchTerm, user.Id);

            return PartialView("_SearchResults", results);
        }

        public async Task<IActionResult> SearchText(string searchTerm)
        {
            var user = await _userManager.GetUserAsync(User);
            user = await _userManager.Users.Include(u => u.Followers).Include(u => u.Followings).Include(u => u.Notifications.OrderByDescending(n => n.NotificationDateTime)).FirstOrDefaultAsync(u => u.Id == user.Id);

            FillViewBags(user);
            var results = ClsSearch.getSearchResult(searchTerm, user.Id);
            ViewBag.titels = ClsUserPosts.getTitleList(results.posts);
            ViewBag.Followings = user.Followings.ToList();
            return View(results);
        }
    }
}
