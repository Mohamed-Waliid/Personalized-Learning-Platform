using System.ComponentModel.DataAnnotations;


namespace Graduation_Project.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Your name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}