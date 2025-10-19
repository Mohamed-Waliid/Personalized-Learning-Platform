using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role Name is required")]
        [Display(Name = "Role Name")]
        [RegularExpression(@"^[a-zA-Z]{3,15}$", ErrorMessage = "Role Name must be 3-15 characters and contain only letters.")]
        public string Name { get; set; }
    }
}
