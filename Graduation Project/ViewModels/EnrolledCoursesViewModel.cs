using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels
{
    public class EnrolledCoursesViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string DifficultyLevel { get; set; }
        public string Duration { get; set; }
        public double Progress { get; set; }
    }
    
}
