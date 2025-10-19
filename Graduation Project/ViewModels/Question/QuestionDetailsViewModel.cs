using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Graduation_Project.Models;

namespace Graduation_Project.ViewModels.Question
{
    public class QuestionDetailsViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string AnswerOptions { get; set; }
        public string? SelectedAnswer { get; set; } // submitted answer
        public string? CorrectAnswer { get; set; }  // from DB
        public bool? IsCorrect { get; set; }        // evaluation
        public int QuizID { get; set; }
        public string QuizName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        //public ICollection<Graduation_Project.Models.Question> Questions { get; set; }
    }

}
