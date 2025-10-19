using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Quiz
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string DifficultyLevel { get; set; }

        // Foreign Key
        [ForeignKey("Course")]
        public int CourseID { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
        public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}