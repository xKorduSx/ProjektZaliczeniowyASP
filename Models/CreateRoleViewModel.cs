
using System.ComponentModel.DataAnnotations;
namespace SportsEquipmentRental.Models
{
	public class CreateRoleViewModel
	{
		[Required]
		[Display(Name = "Role")]
		public string RoleName { get; set; }
	}
}
