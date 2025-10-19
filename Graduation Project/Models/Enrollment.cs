using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Enrollment
    {
        [Key]
        public int ID { get; set; }


        [Required]
        public DateTime EnrollmentDate { get; set; }


        // Foreign Keys
        [ForeignKey("ApplicationUser")]
        public string StudentID { get; set; }


        [ForeignKey("Course")]
        public int CourseID { get; set; }


        // Navigation Properties
        public Course Course { get; set; }
        public ApplicationUser Student { get; set; }
        public ICollection<CompletedMaterial> CompletedMaterials { get; set; } = new List<CompletedMaterial>();
    }
}