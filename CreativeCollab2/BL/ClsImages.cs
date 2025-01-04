//using CreativeCollab2.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace CreativeCollab2.BL
//{
//	public class ClsImages
//	{
//		//private readonly UserManager<ApplicationUser> _userManager;
//		//public ClsImages(UserManager<ApplicationUser> userManager)
//  //      {
//  //          _userManager = userManager;
//  //      }
//        public async Task<bool> EditProfileImage(IFormFile ProfileImage)
//		{
//			string ImageName = await ClsHelper.UploadImage(ProfileImage, "user");
//			if (!string.IsNullOrEmpty(ImageName))
//			{


//				if (user != null)
//				{
//					user.ProfileImage = ImageName;
//					await _userManager.UpdateAsync(user);

//				}
//				return true;
//			}
//			return false;
//		}
//		public async Task<bool> EditCoverImage(IFormFile ProfileImage , ApplicationUser user)
//		{
//			string ImageName = await ClsHelper.UploadImage(ProfileImage, "user");
//			if (!string.IsNullOrEmpty(ImageName))
//			{


//				if (user != null)
//				{
//					user.CoverImage = ImageName;
//					await _userManager.UpdateAsync(user);

//				}
//				return true;
//			}
//			return false;
//		}

//	}
//}
