using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace CreativeCollab2.Controllers
{
	public class RowsGeneratorController : Controller
	{
		UserManager<ApplicationUser> _userManager;
		Irows ClsRows;


		public RowsGeneratorController(UserManager<ApplicationUser> UserManager, Irows i)
		{
			_userManager = UserManager;
			ClsRows = i;

		}
		public async Task<IActionResult> Index()
		{

			if (await ClsRows.generateUsers())
			{
				return Content("<h1>generated success</h1>", "text/html");
			}
			else
			{
				return Content("<h1>generated failed</h1>", "text/html");

			}
		}
		public async Task<IActionResult> t()
		{

			ClsRows.generateLikes();

			return Content("<h1>generated success </h1>", "text/html");

		}
	}
}
