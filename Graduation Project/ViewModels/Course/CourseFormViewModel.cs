using Graduation_Project.Models;
using Graduation_Project.ViewModels.Track;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Graduation_Project.ViewModels.Course
{
    public class CourseFormViewModel
    {
        public int ID { get; set; } // Used for Edit scenarios

        
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 60 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description can be up to 500 characters.")]
        public string Description { get; set; } 

        
        [Required(ErrorMessage = "Difficulty Level is required.")]
        [Display(Name = "Difficulty Level")]
        public string DifficultyLevel { get; set; }

        
        
        [Required(ErrorMessage = "Course duration is required.")]
        public string Duration { get; set; }


        public string? ImageURL { get; set; }


        [Display(Name = "Course Poster")]
        [Required(ErrorMessage = "Please upload an image for the course.")]
        public IFormFile CourseImageFile { get; set; }

        
        
        [Required(ErrorMessage = "Please select at least one track.")]
        [Display(Name = "Select CourseTracks")]
        public List<int> SelectedTrackIds { get; set; } = new();

        public List<Graduation_Project.Models.Track> AvailableTracks { get; set; } = new();
    }
}
