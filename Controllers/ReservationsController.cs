using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsEquipmentRental.Data;
using SportsEquipmentRental.Models;

namespace SportsEquipmentRental.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

		// GET: Reservations
		[Authorize(Roles = "Admin, User")]
		public async Task<IActionResult> Index(bool showSingleUser=true)
        {
			var applicationDbContext = _context.Reservation.Include(r => r.Equipment).Include(r => r.User);
            ViewData["showSingleUser"] = showSingleUser;
            bool isAdmin = User.IsInRole("Admin");
			if (showSingleUser || (!isAdmin))
			{
				var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				return View("Index", await applicationDbContext.Where(u => u.IdentityUserId == currentUserId).ToListAsync());
			}
			else
			{
				return View("Index", await applicationDbContext.ToListAsync());
			}
		}

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Equipment)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

		// GET: Reservations/Create
		public IActionResult Create()
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            ViewData["isAdmin"] = isAdmin;
			//ViewData["isAdmin"] = _context.Users.Find(u => u.Id == currentUserId);
			ViewData["EquipmentId"] = new SelectList(_context.Equipment.Where(e => (e.IsAvailable == true)), "EquipmentId", "Name", _context.Equipment.First().EquipmentId);
            if (isAdmin)
            {
                ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", currentUserId);
			}
			else
			{
				ViewData["IdentityUserId"] = new SelectList(_context.Users.Where(u => u.Id == currentUserId), "Id", "Email", currentUserId);
			}

			List<EquipmentViewModel> EquipmentList = new List<EquipmentViewModel>();
			foreach (var item in _context.Equipment)
			{
				EquipmentViewModel equipmentViewModel = new EquipmentViewModel();
				equipmentViewModel.EquipmentId = item.EquipmentId;
				equipmentViewModel.Name = item.Name;
				equipmentViewModel.RentalPricePerDay = item.RentalPricePerDay;
				EquipmentList.Add(equipmentViewModel);
			}
			ViewData["EquipmentList"] = EquipmentList;
			
			return View();
		}

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdentityUserId,EquipmentId,StartDate,EndDate,TotalPrice,IsPaid")] Reservation reservation)
        {
			bool isAdmin = User.IsInRole("Admin");
			ViewData["isAdmin"] = isAdmin;

			if (ModelState.IsValid)
            {
				bool injectionDetected = false;

				//var startDate = reservation.StartDate;
				//var endDate = reservation.EndDate;
				//var rentalPricePerDay = _context.Equipment.FirstOrDefault(e => e.EquipmentId == reservation.EquipmentId).RentalPricePerDay;

				//var diffTime = Math.Abs(endDate.Subtract(startDate).TotalMicroseconds);
				//var diffDays = Math.Ceiling(diffTime / (1000 * 60 * 60 * 24)); // Difference in days
				//var totalPrice = (decimal) diffDays * rentalPricePerDay;
                //if ( (decimal) totalPrice != reservation.TotalPrice) { injectionDetected = true; }

				if (!injectionDetected)
                {
					_context.Add(reservation);
					var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.EquipmentId == reservation.EquipmentId);
					if (equipment != null)
					{
						equipment.IsAvailable = false;
					}
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
            }

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			ViewData["EquipmentId"] = new SelectList(_context.Equipment.Where(e => (e.IsAvailable == true)), "EquipmentId", "Name");
			ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", currentUserId);

			List<EquipmentViewModel> EquipmentList = new List<EquipmentViewModel>();
			foreach (var item in _context.Equipment)
			{
				EquipmentViewModel equipmentViewModel = new EquipmentViewModel();
				equipmentViewModel.EquipmentId = item.EquipmentId;
				equipmentViewModel.Name = item.Name;
				equipmentViewModel.RentalPricePerDay = item.RentalPricePerDay;
				EquipmentList.Add(equipmentViewModel);
			}
			ViewData["EquipmentList"] = EquipmentList;

			return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			bool isAdmin = User.IsInRole("Admin");
			ViewData["isAdmin"] = isAdmin;
			//ViewData["isAdmin"] = _context.Users.Find(u => u.Id == currentUserId);
			ViewData["EquipmentId"] = new SelectList(_context.Equipment.Where(e => (e.IsAvailable == true || e.EquipmentId == reservation.EquipmentId)), "EquipmentId", "Name", reservation.EquipmentId);
			if (isAdmin)
			{
				ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", reservation.IdentityUserId);
			}
			else
			{
				ViewData["IdentityUserId"] = new SelectList(_context.Users.Where(u => u.Id == currentUserId), "Id", "Email", reservation.IdentityUserId);
			}

			List<EquipmentViewModel> EquipmentList = new List<EquipmentViewModel>();
			foreach (var item in _context.Equipment)
			{
				EquipmentViewModel equipmentViewModel = new EquipmentViewModel();
				equipmentViewModel.EquipmentId = item.EquipmentId;
				equipmentViewModel.Name = item.Name;
				equipmentViewModel.RentalPricePerDay = item.RentalPricePerDay;
				EquipmentList.Add(equipmentViewModel);
			}
			ViewData["EquipmentList"] = EquipmentList;

			return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationId,IdentityUserId,EquipmentId,StartDate,EndDate,TotalPrice,IsPaid")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

			ViewData["EquipmentId"] = new SelectList(_context.Equipment.Where(e => (e.IsAvailable == true || e.EquipmentId == reservation.EquipmentId)), "EquipmentId", "Name", reservation.EquipmentId);
			ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Email", reservation.IdentityUserId);

			List<EquipmentViewModel> EquipmentList = new List<EquipmentViewModel>();
			foreach (var item in _context.Equipment)
			{
				EquipmentViewModel equipmentViewModel = new EquipmentViewModel();
				equipmentViewModel.EquipmentId = item.EquipmentId;
				equipmentViewModel.Name = item.Name;
				equipmentViewModel.RentalPricePerDay = item.RentalPricePerDay;
				EquipmentList.Add(equipmentViewModel);
			}
			ViewData["EquipmentList"] = EquipmentList;

			return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Equipment)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationId == id);
        }
    }
}
