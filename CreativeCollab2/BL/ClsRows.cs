using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace CreativeCollab2.BL
{
	public interface Irows
	{
		public Task<bool> generateUsers();
		public Task<bool> generatePosts();
		public Task<bool> generateLikes();


	}

	public class ClsRows : Irows
	{
		UserManager<ApplicationUser> _userManager;
		CreativeCollabContext ctx;
		Iintersts ClsInterests;

		public ClsRows(UserManager<ApplicationUser> UserManager, CreativeCollabContext _ctx, Iintersts interest)
		{
			_userManager = UserManager;
			ctx = _ctx;
			ClsInterests = interest;

		}

		public async Task<bool> generatePosts()
		{
			throw new NotImplementedException();


		}

		public async Task<bool> generateUsers()
		{
			List<Interest> AllInterstsList = ClsInterests.GetAllIntersts();

			for (int i = 0; i < 100; i++)
			{
				var user = new ApplicationUser()
				{
					FirstName = "User" + i,
					LastName = "Lastname" + i,
					Email = "user" + i + "@example.com",
					Gender = 'M',
					CoverImage = "default_image.png",
					ProfileImage = "DefaultMan.jpg",
					UserName = "user" + i + "@example.com"

				};

				if (i % 2 == 0)
				{
					user.Interests.Add(AllInterstsList[i % 8]);
					user.Interests.Add(AllInterstsList[(i + 1) % 8]);
				}
				else
				{
					user.Interests.Add(AllInterstsList[i % 8]);
					user.Interests.Add(AllInterstsList[(i + 1) % 8]);
					user.Interests.Add(AllInterstsList[(i + 2) % 8]);
				}

				try
				{
					var result = await _userManager.CreateAsync(user, "Password@123");
					ctx.SaveChanges();
					if (result.Succeeded)
					{
						if (user.Interests.Count == 2)
						{

							for (int j = 0; j < 2; j++)
							{
								Image image = new Image()
								{
									ImageName = RandomName(user.Interests.ElementAt(j).InterestName) + ".png",

									InterestId = user.Interests.Select(u => u.InterestId).ElementAt(j),
								};
								ctx.Images.Add(image);
								ctx.SaveChanges();

								var lastUserId = user.Id;

								var ImageId = image.ImageId;

								ctx.Posts.Add(new Post()
								{
									PostText = "post " + user.FirstName + " " + j,
									PostImageId = ImageId,
									UserId = lastUserId,
									GroupId = null,
									IsDeleted = false,
									PostDateTime = DateTime.UtcNow,
								});


							}
							ctx.SaveChanges();


						}
						else
						{
							for (int j = 0; j < 3; j++)
							{

								Image image = new Image()
								{
									ImageName = RandomName(user.Interests.ElementAt(j).InterestName) + ".png",

									InterestId = user.Interests.Select(u => u.InterestId).ElementAt(j),
								};
								ctx.Images.Add(image);
								ctx.SaveChanges();

								var lastUserId = user.Id;

								var ImageId = image.ImageId;


								ctx.Posts.Add(new Post()
								{
									PostText = "post " + user.FirstName + " " + j,
									PostImageId = ImageId,
									UserId = lastUserId,
									GroupId = null,
									IsDeleted = false,
									PostDateTime = DateTime.UtcNow,
								});



							}
							ctx.SaveChanges();

						}
					}
					else
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					return false;
				}

			}
			return true;
		}

		private HashSet<string> generatedNames = new HashSet<string>();

		public string RandomName(string category)
		{
			Random rand = new Random();
			string name;

			do
			{
				int number = rand.Next(1, 101);
				name = $"{category}{number}";
			} while (generatedNames.Contains(name));

			generatedNames.Add(name);
			return name;
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
		public async Task<bool> generateLikes()
		{
			var users = ctx.Users.Include(u => u.Interests).ToList();
			var posts = ctx.Posts.Include(p => p.PostImage).ThenInclude(u => u.Interest).ToList();
			int counter = 0;
			for (int i = 0; i < users.Count; i++)
			{
				posts = Shuffle(posts);

				foreach (var post in posts)
				{
					var postInterestId = (int)(post.PostImage.InterestId);
					if ((users[i].Interests.Select(u => u.InterestId).ToList().Contains((int)post.PostImage.InterestId)) && (post.UserId != users[i].Id))
					{

						if (counter < 50)
						{
							var UserLikes = ctx.Likes.Include(p => p.Post).ThenInclude(p => p.PostImage).Where(l => l.UserId == users[i].Id).ToList();
							var t = UserLikes.Where(p => p.Post.PostImage.InterestId == postInterestId).FirstOrDefault();
							if (t != null)
							{

								continue;
							}
							else
							{
								ctx.Likes.Add(new Like()
								{
									PostId = post.PostId,
									UserId = users[i].Id
								});

							}
							ctx.SaveChanges();

						}
						else
						{
							ctx.Likes.Add(new Like()
							{
								PostId = post.PostId,
								UserId = users[i].Id
							});
							ctx.SaveChanges();
						}
					}
				}
				counter++;
			}
			return true;
		}

	}
}
