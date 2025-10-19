using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Recommendation
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Track")]
        public int TrackID { get; set; }

        [Required]
        public string Skills { get; set; }

        public int? Feedback { get; set; }

        // Foreign Key
        [ForeignKey("ApplicationUser")]
        public string StudentID { get; set; }

        // Navigation Property
        public Track Track { get; set; }
        public ApplicationUser Student { get; set; }
    }
}