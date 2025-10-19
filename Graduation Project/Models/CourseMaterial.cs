using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class CourseMaterial
    {
        [Key]
        public int ID { get; set; }


        [ForeignKey("Course")]
        public int CourseID { get; set; }



        [ForeignKey("LearningMaterial")]
        public int MaterialID { get; set; }



        // Navigation
        public Course Course { get; set; }
        public LearningMaterial LearningMaterial { get; set; }
    }
}
