using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Question
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string AnswerOptions { get; set; }

        [Required]
        public string CorrectAnswer { get; set; }

        // Foreign Key
        [ForeignKey("Quiz")]
        public int QuizID { get; set; }

        // Navigation Property
        public Quiz Quiz { get; set; }
    }
}

