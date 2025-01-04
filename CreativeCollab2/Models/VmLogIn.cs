using System.ComponentModel.DataAnnotations;

namespace CreativeCollab2.Models
{
	public class VmLogIn
	{
		[Required(ErrorMessage = "Email Address field is required.")]
		[EmailAddress(ErrorMessage = "invalid email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password field is required.")]
		public string Password { get; set; }
        public bool RememberMe { get; set; }
	}
}
