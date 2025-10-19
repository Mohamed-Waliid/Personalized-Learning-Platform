using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Course
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string Duration { get; set; }


        [Required]
        public string DifficultyLevel { get; set; }

        [Required]
        public string ImagePath { get; set; } // e.g., "/images/courses/course1.jpg"



        // Foreign Key
        [ForeignKey("ApplicationUser")]
        public string InstructorID { get; set; }


        // Navigation Properties
        public ApplicationUser Instructor { get; set; }
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<CourseTrack> CourseTracks { get; set; } = new List<CourseTrack>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
    }
}