using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Track
{
    public class TrackViewModel
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters")]
        [Required(ErrorMessage = "Track name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Track description is required")]
        public string Description { get; set; }

        [Display(Name = "Track Poster")]
        public IFormFile? TrackImageFile { get; set; }

        public string? ImageURL { get; set; }
    }
}
