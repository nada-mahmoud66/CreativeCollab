using CreativeCollab2.Models;
using Microsoft.EntityFrameworkCore;

namespace CreativeCollab2.BL
{
	public interface Iintersts
	{
		public Interest getInterest(int interestId);
		public List<Interest> GetAllIntersts();
		public ApplicationUser getUserWithInterests(string userEmail);
	}
	public class ClsIntersts : Iintersts
	{
		CreativeCollabContext ctx;
		public ClsIntersts(CreativeCollabContext _context)
		{
			ctx = _context;

		}
		public Interest getInterest(int interestId)
		{
			return ctx.Interests.Where(I => I.InterestId == interestId).FirstOrDefault();
		}
		public List<Interest> GetAllIntersts()
		{
			return ctx.Interests.ToList();

		}
		public ApplicationUser getUserWithInterests(string userEmail)
		{
			return ctx.Users.Include(u => u.Interests).Where(U => U.Email == userEmail).FirstOrDefault();
		}
	}
}
