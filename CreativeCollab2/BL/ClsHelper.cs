using CreativeCollab2.Models;

namespace CreativeCollab2.BL
{

	public static class ClsHelper
	{

		public static string UploadImage(IFormFile ProfileImage, string FolderName)
		{

			string ImageName = "";
			if (ProfileImage.Length > 0 && ProfileImage != null)
			{
				var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", FolderName);
				ImageName = Guid.NewGuid().ToString() + ".png";
				// Define the file path
				var filePath = Path.Combine(uploadsDirectory, ImageName);

				// Copy the contents of the uploaded photo to a FileStream
				var stream = new FileStream(filePath, FileMode.Create);
				ProfileImage.CopyTo(stream);
			}

			return ImageName;
		}

		//public async Task<bol> FillViewBags(ApplicationUser user)
		//{
		//	user.Followers = await ClsFollow.GetFollowersListbyId(userId);
		//	user.Followings = await ClsFollow.GetFollowingListbyId(userId);

		//	ViewBag.ProfileImage = user.ProfileImage;
		//	ViewBag.CoverImage = user.CoverImage;
		//	ViewBag.NumOfFollowers = user.Followers.Count;
		//	ViewBag.FirstName = user.FirstName;
		//	ViewBag.LastName = user.LastName;


		//}
	}
}
