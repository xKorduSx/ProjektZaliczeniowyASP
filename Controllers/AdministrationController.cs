﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsEquipmentRental.Models;

namespace SportsEquipmentRental.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdministrationController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;

		public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}




		[HttpGet]
		public IActionResult CreateRole()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
		{
			if (ModelState.IsValid)
			{
				// Check if the role already exists
				bool roleExists = await _roleManager.RoleExistsAsync(roleModel?.RoleName);
				if (roleExists)
				{
					ModelState.AddModelError("", "Role Already Exists");
				}
				else
				{
					// Create the role
					// We just need to specify a unique role name to create a new role
					IdentityRole identityRole = new IdentityRole
					{
						Name = roleModel?.RoleName
					};

					// Saves the role in the underlying AspNetRoles table
					IdentityResult result = await _roleManager.CreateAsync(identityRole);

					if (result.Succeeded)
					{
						return RedirectToAction("ListRoles", "Administration");
					}

					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return View(roleModel);
		}


		[HttpGet]
		public async Task<IActionResult> ListRoles()
		{
			List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}




		[HttpGet]
		public async Task<IActionResult> EditRole(string roleId)
		{
			//First Get the role information from the database
			IdentityRole? role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				// Handle the scenario when the role is not found
				return View("Error");
			}

			//Populate the EditRoleViewModel from the data retrived from the database
			var model = new EditRoleViewModel
			{
				Id = role.Id,
				RoleName = role.Name,
				Users = new List<string>()
				// You can add other properties here if needed
			};

			// Retrieve all the Users
			foreach (var user in _userManager.Users.ToList())
			{
				// If the user is in this role, add the username to
				// Users property of EditRoleViewModel. 
				// This model object is then passed to the view for display
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					model.Users.Add(user.UserName);
				}
			}

			return View(model);
		}






		[HttpPost]
		public async Task<IActionResult> EditRole(EditRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				var role = await _roleManager.FindByIdAsync(model.Id);
				if (role == null)
				{
					// Handle the scenario when the role is not found
					ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
					return View("NotFound");
				}
				else
				{
					role.Name = model.RoleName;
					// Update other properties if needed

					var result = await _roleManager.UpdateAsync(role);
					if (result.Succeeded)
					{
						return RedirectToAction("ListRoles"); // Redirect to the roles list
					}

					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

					return View(model);
				}
			}

			return View(model);
		}







		[HttpPost]
		public async Task<IActionResult> DeleteRole(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				// Role not found, handle accordingly
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("NotFound");
			}

			var result = await _roleManager.DeleteAsync(role);
			if (result.Succeeded)
			{
				// Role deletion successful
				return RedirectToAction("ListRoles"); // Redirect to the roles list page
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			// If we reach here, something went wrong, return to the view
			return View("ListRoles", await _roleManager.Roles.ToListAsync());
		}








		[HttpGet]
		public async Task<IActionResult> EditUsersInRole(string roleId)
		{
			ViewBag.roleId = roleId;

			var role = await _roleManager.FindByIdAsync(roleId);

			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("NotFound");
			}

			ViewBag.RollName = role.Name;
			var model = new List<UserRoleViewModel>();

			foreach (var user in _userManager.Users.ToList())
			{
				var userRoleViewModel = new UserRoleViewModel
				{
					UserId = user.Id,
					UserName = user.UserName
				};

				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRoleViewModel.IsSelected = true;
				}
				else
				{
					userRoleViewModel.IsSelected = false;
				}

				model.Add(userRoleViewModel);
			}

			return View(model);
		}




		[HttpPost]
		public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
		{
			//First check whether the Role Exists or not
			var role = await _roleManager.FindByIdAsync(roleId);

			if (role == null)
			{
				ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
				return View("NotFound");
			}

			for (int i = 0; i < model.Count; i++)
			{
				var user = await _userManager.FindByIdAsync(model[i].UserId);

				IdentityResult? result;

				if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
				{
					//If IsSelected is true and User is not already in this role, then add the user
					result = await _userManager.AddToRoleAsync(user, role.Name);
				}
				else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
				{
					//If IsSelected is false and User is already in this role, then remove the user
					result = await _userManager.RemoveFromRoleAsync(user, role.Name);
				}
				else
				{
					//Don't do anything simply continue the loop
					continue;
				}

				//If you add or remove any user, please check the Succeeded of the IdentityResult
				if (result.Succeeded)
				{
					if (i < (model.Count - 1))
						continue;
					else
						return RedirectToAction("EditRole", new { roleId = roleId });
				}
			}

			return RedirectToAction("EditRole", new { roleId = roleId });
		}







		[HttpGet]
		public IActionResult ListUsers()
		{
			var users = _userManager.Users;
			return View(users);
		}






		[HttpPost]
		public async Task<IActionResult> DeleteUser(string UserId)
		{
			//First Fetch the User you want to Delete
			var user = await _userManager.FindByIdAsync(UserId);

			if (user == null)
			{
				// Handle the case where the user wasn't found
				ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
				return View("NotFound");
			}
			else
			{
				//Delete the User Using DeleteAsync Method of UserManager Service
				var result = await _userManager.DeleteAsync(user);

				if (result.Succeeded)
				{
					// Handle a successful delete
					return RedirectToAction("ListUsers");
				}
				else
				{
					// Handle failure
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}

				return View("ListUsers");
			}
		}



	}
}
