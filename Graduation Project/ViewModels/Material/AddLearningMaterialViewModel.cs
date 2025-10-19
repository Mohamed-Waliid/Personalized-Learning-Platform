using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Material
{
    public class AddLearningMaterialViewModel
    {
        public int ID { get; set; }
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Title must be 3-20 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Type must be 3-20 characters.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Content must be 3-300 characters.")]
        public string Url { get; set; }

        [ValidateNever]
        public Models.Course course { get; set; }
    }
}