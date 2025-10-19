using Graduation_Project.Models;
using Graduation_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Graduation_Project.Data;


namespace Graduation_Project.Controllers
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }


    public class AccountController : Controller
    {
        private readonly ApplicationDBContext ctx;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly UserManager<ApplicationUser> userManager;
        public readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, ApplicationDBContext ct)
        {
            ctx = ct;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task bulkinsert()
        {
            string csvFilePath = "D:/VSC/misc/pythonn/students.csv";
            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });

            var users = csv.GetRecords<UserModel>().ToList();
            foreach (var user in users)
            {
                var appUser = new ApplicationUser()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber
                };

                var result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, user.Role);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
        }

        public async Task<IActionResult> Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            var roles = await roleManager.Roles.ToListAsync();

            foreach (var item in roles)
            {
                if (item.Name != "Admin")
                    model.Roles.Add(item);
            }
            return View("Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegister(RegisterViewModel model)
        {
            var roles = await roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                if (role.Name != "Admin")
                {
                    model.Roles.Add(role);  // dah 34an el validation cases lma arg3 el view tane el list bta3t el roles mtb2a4 fadya
                }
            }

            if (ModelState.IsValid)
            {
                var ExistingUser = await userManager.FindByEmailAsync(model.Email);

                if (ExistingUser != null)
                {
                    ModelState.AddModelError("Email", "This email address is already taken.");
                    return View("Register", model);
                }

                ApplicationUser appUser = new ApplicationUser();
                appUser.Email = model.Email;
                appUser.UserName = model.Email;
                appUser.FullName = model.FullName;
                appUser.PhoneNumber = model.PhoneNumber;

                IdentityResult result = await userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, model.SelectedRole);
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            return View("Register", model);
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = await userManager.FindByEmailAsync(model.Email);

                if (appUser != null)
                {
                    bool passwordOk = await userManager.CheckPasswordAsync(appUser, model.Password);

                    if (passwordOk)
                    {
                        await signInManager.SignInAsync(appUser, model.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Password", "Email or Password is wrong");
            }
            return View("Login", model);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile(string ID)
        {
            var user = await userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound();
            }

            List<IdentityRole> Roles = await ctx.Roles
                .AsNoTracking()
                .Where(r => ctx.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == ID)
                    .Select(ur => ur.RoleId)
                    .Contains(r.Id)
                )
                .ToListAsync();

            ProfileViewModel obj = new ProfileViewModel()
            {
                ID = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePic = user.ProfilePicture,
                Email = user.Email,
                Roles = Roles
            };

            return View("profile", obj);
        }

        [Authorize]
        public IActionResult ChangePassword(string ID)
        {
            ChangePasswordViewModel obj = new ChangePasswordViewModel()
            {
                ID = ID
            };

            return View(obj);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SavePassChange(ChangePasswordViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View("changepassword", obj);
            }

            var user = await userManager.FindByIdAsync(obj.ID);

            var result = await userManager.ChangePasswordAsync(user, obj.OldPassword, obj.NewPassword);

            if (result.Succeeded)
            {
                await signInManager.RefreshSignInAsync(user); // optional but keeps session alive
                return RedirectToAction("Profile", new { ID = user.Id });
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
                ModelState.AddModelError("", error.Description);
            }

            return View("changepassword", obj);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string ID)
        {
            var user = await userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel obj = new EditUserViewModel();
            obj.ID = user.Id;
            obj.FullName = user.FullName;
            obj.Email = user.Email;
            obj.PhoneNumber = user.PhoneNumber;
            obj.SelectedRole = await ctx.Roles
                .AsNoTracking()
                .Where(
                    r => ctx.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == ID)
                    .Select(ur => ur.RoleId)
                    .Contains(r.Id)
                )
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            var roles = await roleManager.Roles.ToListAsync();

            foreach (var item in roles)
            {
                if (item.Name != "Admin")
                {
                    obj.Roles.Add(item);
                }
            }

            return View("Edit", obj);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(EditUserViewModel obj)
        {
            var roles = await roleManager.Roles.ToListAsync();
            var user = await userManager.FindByIdAsync(obj.ID);

            foreach (var role in roles)
            {
                if (role.Name != "Admin")
                {
                    obj.Roles.Add(role);  // dah 34an el validation cases lma arg3 el view tane el list bta3t el roles mtb2a4 fadya
                }
            }

            if (ModelState.IsValid)
            {
                user.Email = obj.Email;
                user.UserName = obj.Email;
                user.FullName = obj.FullName;
                user.PhoneNumber = obj.PhoneNumber;

                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await userManager.RemoveFromRoleAsync(user, obj.SelectedRole == "Instructor" ? "Student" : "Instructor");
                    await userManager.AddToRoleAsync(user, obj.SelectedRole);
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Profile", new { obj.ID });
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }

            return View("Edit", obj);
        }
    }
}