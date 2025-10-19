using System.ComponentModel.DataAnnotations;


namespace Graduation_Project.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string? ID { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}