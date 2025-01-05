namespace SportsEquipmentRental.Models
{
	public class Equipment
	{
		public int EquipmentId { get; set; }
		public string Name { get; set; }
		public string Category { get; set; } // e.g., "Ski", "Bike"
		public bool IsAvailable { get; set; }
		public decimal RentalPricePerDay { get; set; }

		// Navigation property
		public ICollection<Reservation>? Reservations { get; set; }
	}
}
