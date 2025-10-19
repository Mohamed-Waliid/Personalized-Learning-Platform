using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class CourseTrack
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Track")]
        public int TrackID { get; set; }


        [ForeignKey("Course")]
        public int CourseID { get; set; }


        // Navigation 
        public Track Track { get; set; }
        public Course Course { get; set; }
    }
}
