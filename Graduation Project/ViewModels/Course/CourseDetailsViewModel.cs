using Graduation_Project.ViewModels.Material;
using Graduation_Project.ViewModels.Project;
using Graduation_Project.ViewModels.Quiz;
using Graduation_Project.Models;

namespace Graduation_Project.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool isOwner { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ApplicationUser Instructor { get; set; }
        public string Duration { get; set; }
        public string DifficultyLevel { get; set; }
        public int EnrolledCount { get; set; }
        public List<string> Tracks { get; set; } 
        public bool IsEnrolled { get; set; }
        public double Progress { get; set; } 
        public List<MaterialViewModel> Materials { get; set; }
        public List<QuizViewModel> Quizzes { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
    }
}