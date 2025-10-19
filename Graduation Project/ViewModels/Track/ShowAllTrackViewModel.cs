using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Track
{
    public class ShowAllTrackViewModel
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }


        [Display(Name = "Track Poster")]
        public IFormFile TrackImageFile { get; set; }

        public string? ImageURL { get; set; }

        public int StudentCount { get; set; }

        public int CourseCount { get; set; }
    }
}
