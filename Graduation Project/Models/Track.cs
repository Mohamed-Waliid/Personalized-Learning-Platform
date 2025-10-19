using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Track
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageURL { get; set; }

        [Required]
        public string Description { get; set; }

        // Navigation Property
        public ICollection<CourseTrack> CourseTracks { get; set; } = new List<CourseTrack>();
    }
}
