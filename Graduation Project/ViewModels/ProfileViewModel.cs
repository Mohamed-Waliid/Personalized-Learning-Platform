using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Graduation_Project.ViewModels
{
    public class ProfileViewModel
    {
        public string? ID { get; set; }

        [Display(Name = "Full Name")]
        [RegularExpression(@"^[a-zA-Z\s]{3,30}$", ErrorMessage = "Full Name must contain only letters and be between 3 and 30 characters.")]
        public string UserName { get; set; }

        public string? ProfilePic { get; set; }

        [Display(Name = "Full Name")]
        [RegularExpression(@"^[a-zA-Z\s]{3,30}$", ErrorMessage = "Full Name must contain only letters and be between 3 and 30 characters.")]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits.")]
        public string PhoneNumber { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}