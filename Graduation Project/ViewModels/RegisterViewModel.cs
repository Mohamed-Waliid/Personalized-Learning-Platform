using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Graduation_Project.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Roles = new List<IdentityRole>();
        }

        public string? ID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^[a-zA-Z\s]{3,30}$", ErrorMessage = "Full Name must contain only letters and be between 3 and 30 characters.")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Phone Number is required.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Please select a role.")]
        [Display(Name = "Register as")]
        public string SelectedRole { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
