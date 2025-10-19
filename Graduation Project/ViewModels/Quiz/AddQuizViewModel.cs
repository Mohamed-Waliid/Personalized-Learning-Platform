using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Quiz
{
    public class AddQuizViewModel
    {
        public int ID { get; set; }

        public int CourseID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{3,20}$", ErrorMessage = "Title must be 3-20 characters")]
        public string Title { get; set; }

        [Display(Name = "Difficulty Level")]
        [Required(ErrorMessage = "Difficulty Level is required")]
        public string DifficultyLevel { get; set; }

        [ValidateNever]
        public Models.Course course { get; set; }
    }
}
