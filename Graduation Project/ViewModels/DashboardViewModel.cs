using Graduation_Project.Models;

namespace Graduation_Project.ViewModels
{
    public class DashboardViewModel
    {
        public List<Course.ShowAllCourseViewModel>? Courses { get; set; }
        public Recommendation? LastRecommendation { get; set; }
        public List<Tuple<Course.ShowAllCourseViewModel, int>>? CoursesProgress { get; set; }
    }
}