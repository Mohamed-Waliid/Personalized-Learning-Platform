using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Project
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string DifficultyLevel { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        // Navigation Property
        public Course Course { get; set; }
    }
}