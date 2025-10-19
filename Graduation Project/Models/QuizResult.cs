using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class QuizResult
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public DateTime AttemptDate { get; set; }

        [ForeignKey("ApplicationUser")]
        public string StudentID { get; set; }

        [ForeignKey("Quiz")]
        public int QuizID { get; set; }

        // Navigation Properties
        public Quiz Quiz { get; set; }
        public ApplicationUser Student { get; set; }
    }
}