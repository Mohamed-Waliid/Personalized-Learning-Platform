using Graduation_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class CompletedMaterial
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Enrollment")]
        public int EnrollmentID { get; set; }


        [ForeignKey("Material")]
        public int MaterialID { get; set; }


        // Navigation Properties
        public Enrollment Enrollment { get; set; }
        public LearningMaterial Material { get; set; }
    }
}