using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsEquipmentRental.Models;

namespace SportsEquipmentRental.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SportsEquipmentRental.Models.Equipment> Equipment { get; set; } = default!;
        public DbSet<SportsEquipmentRental.Models.Reservation> Reservation { get; set; } = default!;
    }
}
