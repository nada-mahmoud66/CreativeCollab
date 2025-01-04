using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CreativeCollab2.Controllers
{
	public class UserController : Controller
	{
		UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		Iintersts ClsIntersts;
		public UserController(Iintersts intersts, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			ClsIntersts = intersts;
		}
		public IActionResult SignUp()
		{

			return View(new VmSignUp());
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(VmSignUp model)
		{
			if (!ModelState.IsValid)
				return View("SignUp", model);

			ApplicationUser user;
			if (model.Gender == 'F')
			{
				user = new ApplicationUser()
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Gender = model.Gender,
					BirthDate = model.BirthDate,
					CoverImage = "default_image.png",
					ProfileImage = "DefaultWomen.jpg",
					UserName = model.Email
				};
			}
			else
			{
				user = new ApplicationUser()
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Gender = model.Gender,
					BirthDate = model.BirthDate,
					CoverImage = "img",
					ProfileImage = "DefaultMan.jpg",
					UserName = model.Email
				};
			}
			try
			{
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					var loginReult = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
					if (loginReult.Succeeded)
						return Redirect("/User/Interests");
				}
				else
				{
					var passwordErrors = new StringBuilder();
					foreach (var error in result.Errors)
					{
						if (error.Code == "PasswordTooShort" || error.Code == "PasswordRequiresNonAlphanumeric" || error.Code == "PasswordRequiresUpper")
						{
							passwordErrors.AppendLine(error.Description);
						}
						else if (error.Code == "DuplicateEmail")
						{
							ModelState.AddModelError("Email", error.Description);
						}

					}
					if (passwordErrors.Length > 0)
					{
						ModelState.AddModelError("Password", passwordErrors.ToString());
					}
				}
			}
			catch (Exception ex) { }
			return View();
		}
		public IActionResult Login()
		{

			return View(new VmLogIn());
		}
		[HttpPost]
		public async Task<IActionResult> Login(VmLogIn model)
		{
			if (!ModelState.IsValid)
				return View("Login", model);
			ApplicationUser user = new ApplicationUser()
			{
				UserName = model.Email,
				Email = model.Email
			};
			try
			{
				var loginReult = await _signInManager.PasswordSignInAsync(user.Email, model.Password, true, true);
				if (loginReult.Succeeded)
				{
					var loggedUser = ClsIntersts.getUserWithInterests(model.Email);
					if (loggedUser.Interests.Count == 0)
						return RedirectToAction("Interests");
					else
						return Redirect("/TimeLine/Index");
				}
				else
					ViewBag.ValidationMessage = "Invalid Email or Password";
			}
			catch (Exception ex) { }
			return View();
		}
		public IActionResult Interests()
		{
			VmInterests vm = new VmInterests();
			vm.AllInterests = ClsIntersts.GetAllIntersts();
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Interests(VmInterests model)
		{
			model.AllInterests = ClsIntersts.GetAllIntersts();
			if (!ModelState.IsValid ||  model.SelectedInterests.Count ==0)
			{
				ViewBag.ValidationMessage = "You must select at least one interest.";
				//model.AllInterests = ClsIntersts.GetAllIntersts();
				return View("Interests", model);
			}

			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user != null)
				{
					//user.Interests = new List<Interest>();
					foreach (int interestId in model.SelectedInterests)
					{
						user.Interests.Add(ClsIntersts.getInterest(interestId));
					}

					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						return Redirect("/TimeLine/Index");
					}
					else
					{
						// Failed to update profile
					}
				}
				else
				{
					// User not found
				}

			}
			catch (Exception ex) { }
			return Redirect("/UserProfile/Index");
		}
	}
}
