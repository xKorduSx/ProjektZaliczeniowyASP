using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsEquipmentRental.Data;
using System.ComponentModel.DataAnnotations;
using NuGet.Packaging;
using SportsEquipmentRental.Models;

namespace WypozyczalniaSprzetuSportowego.Models;

public static class SeedData
{
	public static async Task Initialize(IServiceProvider serviceProvider)
	{
		var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");

		try
		{
			using (var context = new ApplicationDbContext(
				serviceProvider.GetRequiredService<
					DbContextOptions<ApplicationDbContext>>()))
			{
				if (context == null || context.Equipment == null)
				{
					throw new ArgumentNullException("Null ApplicationDbContext");
				}

				var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

				logger.LogInformation("RoleManager and UserManager services retrieved successfully.");

				string[] roleNames = { "Admin", "User" };
				IdentityResult roleResult;

				foreach (var roleName in roleNames)
				{
					var roleExist = await roleManager.RoleExistsAsync(roleName);
					if (!roleExist)
					{
						roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
						logger.LogInformation($"Role '{roleName}' created successfully.");
					}
				}

				// Look for any Equipment.
				if (!context.Equipment.Any())
				{
					context.Equipment.AddRange(
						new Equipment { Name = "Ski Model A", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 20.00m },
						new Equipment { Name = "Ski Model B", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 22.00m },
						new Equipment { Name = "Ski Model C", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 24.00m },
						new Equipment { Name = "Bike Model X", Category = "Cycling", IsAvailable = true, RentalPricePerDay = 15.00m },
						new Equipment { Name = "Bike Model Y", Category = "Cycling", IsAvailable = true, RentalPricePerDay = 18.00m },
						new Equipment { Name = "Bike Model Z", Category = "Cycling", IsAvailable = true, RentalPricePerDay = 20.00m },
						new Equipment { Name = "Tent Model T1", Category = "Camping", IsAvailable = true, RentalPricePerDay = 10.00m },
						new Equipment { Name = "Tent Model T2", Category = "Camping", IsAvailable = true, RentalPricePerDay = 12.00m },
						new Equipment { Name = "Tent Model T3", Category = "Camping", IsAvailable = true, RentalPricePerDay = 14.00m },
						new Equipment { Name = "Snowboard Model S1", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 25.00m },
						new Equipment { Name = "Snowboard Model S2", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 27.00m },
						new Equipment { Name = "Snowboard Model S3", Category = "Winter Sports", IsAvailable = true, RentalPricePerDay = 30.00m },
						new Equipment { Name = "Kayak Model K1", Category = "Water Sports", IsAvailable = true, RentalPricePerDay = 35.00m },
						new Equipment { Name = "Kayak Model K2", Category = "Water Sports", IsAvailable = true, RentalPricePerDay = 40.00m },
						new Equipment { Name = "Kayak Model K3", Category = "Water Sports", IsAvailable = true, RentalPricePerDay = 45.00m }
					);
					context.SaveChanges();
					logger.LogInformation("Equipment seeded successfully.");
				}

				// Look for any Users.
				if (!context.Users.Any())
				{
					var adminUsers = new List<IdentityUser>
						{
							new IdentityUser { UserName = "admin1@example.com", Email = "admin1@example.com", EmailConfirmed = true },
							new IdentityUser { UserName = "admin2@example.com", Email = "admin2@example.com", EmailConfirmed = true },
                           
                        };
					string adminPassword = "Admin@123";

					foreach (var adminUser in adminUsers)
					{
						var user = await userManager.FindByEmailAsync(adminUser.Email ?? string.Empty);
						if (user == null)
						{
							var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
							if (createAdminUser.Succeeded)
							{
								await userManager.AddToRoleAsync(adminUser, "Admin");
								logger.LogInformation($"Admin user '{adminUser.UserName}' created successfully.");
							}
						}
					}

					var normalUsers = new List<IdentityUser>
						{
							new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com", EmailConfirmed = true },
							new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com", EmailConfirmed = true },
							new IdentityUser { UserName = "user3@example.com", Email = "user3@example.com", EmailConfirmed = true }
						};
					string userPassword = "User@123";

					foreach (var normalUser in normalUsers)
					{
						var user = await userManager.FindByEmailAsync(normalUser.Email ?? string.Empty);
						if (user == null)
						{
							var createNormalUser = await userManager.CreateAsync(normalUser, userPassword);
							if (createNormalUser.Succeeded)
							{
								await userManager.AddToRoleAsync(normalUser, "User");
								logger.LogInformation($"Normal user '{normalUser.UserName}' created successfully.");
							}
						}
					}
				}

				// Look for any Reservations.
				if (!context.Reservation.Any())
				{
					var users = userManager.Users.ToList();
					var equipmentList = context.Equipment.ToList();

					context.Reservation.AddRange(
						new Reservation { IdentityUserId = users[0].Id, EquipmentId = equipmentList[0].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(3), TotalPrice = equipmentList[0].RentalPricePerDay * 3, IsPaid = true },
						new Reservation { IdentityUserId = users[1].Id, EquipmentId = equipmentList[1].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TotalPrice = equipmentList[1].RentalPricePerDay * 2, IsPaid = true },
						new Reservation { IdentityUserId = users[2].Id, EquipmentId = equipmentList[2].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(4), TotalPrice = equipmentList[2].RentalPricePerDay * 4, IsPaid = true },
						new Reservation { IdentityUserId = users[0].Id, EquipmentId = equipmentList[3].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TotalPrice = equipmentList[3].RentalPricePerDay * 1, IsPaid = true },
						new Reservation { IdentityUserId = users[1].Id, EquipmentId = equipmentList[4].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), TotalPrice = equipmentList[4].RentalPricePerDay * 5, IsPaid = true },
						new Reservation { IdentityUserId = users[2].Id, EquipmentId = equipmentList[5].EquipmentId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(6), TotalPrice = equipmentList[5].RentalPricePerDay * 6, IsPaid = true }
					);
					context.SaveChanges();
					logger.LogInformation("Reservations seeded successfully.");
				}
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while seeding the database.");
		}
	}
}
