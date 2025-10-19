using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Graduation_Project.Controllers
{
    public class RolesController : Controller
    {
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly UserManager<ApplicationUser> userManager;
        public readonly ApplicationDBContext context;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDBContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveCreate(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await roleManager.RoleExistsAsync(model.Name);

                if (roleExist)
                {
                    ModelState.AddModelError("", "Role already exists");
                    return View("Create", model);
                }

                IdentityRole role = new IdentityRole() { Name = model.Name };
                //role.Name = model.Name;

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View("Create", model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string RoleId)
        {
            return View("Delete", await roleManager.FindByIdAsync(RoleId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmDelete(string ID)
        {
            IdentityRole role = await roleManager.FindByIdAsync(ID);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignAdminRole(string UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);

            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["Message"] = "User already has the Admin role.";
            }
            else
            {
                await userManager.AddToRoleAsync(user, "Admin");
                TempData["Message"] = "Admin role assigned successfully.";
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RevokeAdminRole(string Id)
        {
            await userManager.RemoveFromRoleAsync(await userManager.FindByIdAsync(Id), "Admin");
            TempData["Message"] = "User removed from Admin role ";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}