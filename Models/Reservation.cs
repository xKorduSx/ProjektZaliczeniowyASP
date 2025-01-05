using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SportsEquipmentRental.Models
{
	// Model for Reservation
	public class Reservation
	{
		public int ReservationId { get; set; }
		public string IdentityUserId { get; set; }
		public int EquipmentId { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
		public DateTime StartDate { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
		public DateTime EndDate { get; set; }
		public decimal TotalPrice { get; set; }
		public bool IsPaid { get; set; } // Indicates if the reservation has been paid

		// Navigation properties
		public IdentityUser? User { get; set; }
		public Equipment? Equipment { get; set; }
	}
}
