using System.ComponentModel.DataAnnotations;

namespace CreativeCollab2.Models
{
	public class LoginViewModel
	{
		[EmailAddress(ErrorMessage = "invalid email")]
		public string email { get; set; }
		[Required(ErrorMessage = "The password is required")]
		[DataType(DataType.Password)]
		public string password { get; set; }
		public bool RememberMe { get; set; }
	}
}
