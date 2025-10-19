using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.ViewModels
{
    public class ShowAllViewModel
    {
        public List<IdentityRole> Roles { get; set; }
        public List<ApplicationUser> Students { get; set; }
        public List<ApplicationUser> Instructors { get; set; }
        public List<ApplicationUser> Admins { get; set; }

    }
}
