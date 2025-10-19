using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Question
{
    public class QuestionFormViewModel
    {
        public int ID { get; set; }  // For Edit scenario

        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Question must be between 5 and 300 characters.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Correct answer is required.")]
        [Display(Name = "Correct Answer")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Correct answer must be between 1 and 300 characters.")]
        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "Answer options are required.")]
        [Display(Name = "Answer Options")]
        [StringLength(1000, MinimumLength = 7, ErrorMessage = "Answer options must be 7–1000 characters long. Separate each option with a comma.")]
        public string AnswerOptions { get; set; }

        [Required]
        public int QuizID { get; set; }

        //[Display(Name = "Quiz Name")]
        //public string QuizName { get; set; }
    }
}
