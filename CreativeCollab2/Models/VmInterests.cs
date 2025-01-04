using CreativeCollab2.BL;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CreativeCollab2.Models
{

	public class VmInterests
	{

		public VmInterests()
		{

			SelectedInterests = new List<int>();
		}
		[Required]
		public List<int> SelectedInterests { get; set; }
		[ValidateNever]
		public List<Interest> AllInterests { get; set; }


	}
}
