using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CreativeCollab2.Models
{
	public class VmSignUp
	{
		public VmSignUp()
		{
			BirthDate = DateTime.Now;
		}
		[Required(ErrorMessage = "First Name field is required.")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Last Name field is required.")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Address field is required.")]
		[EmailAddress(ErrorMessage = "invalid email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password field is required.")]
		public string Password { get; set; }
		public char Gender { get; set; }
		[Required(ErrorMessage = "Birthdate field is required.")]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }

	}
}
