namespace CreativeCollab2.Models
{
	public class vmCompoUserAndPosts
	{
		public vmCompoUserAndPosts()
		{
			vmUserPosts = new List<vmUserPosts>();
		}
		public List<vmUserPosts> vmUserPosts { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

		public string Titels { get; set; }
	}
}
