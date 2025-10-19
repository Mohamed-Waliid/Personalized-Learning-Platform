using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.ViewModels.Project
{
    public class AddProjectViewModel
    {
        public int ID { get; set; }

        public int CourseID { get; set; }

        [Required(ErrorMessage = "Project title is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{3,20}$", ErrorMessage = "Title must be 3-20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Project description is required.")]
        public string Description { get; set; }

        [Display(Name = "Difficulty Level")]
        [Required(ErrorMessage = "Project Difficulty level is required.")]
        public string DifficultyLevel { get; set; }
    }
}