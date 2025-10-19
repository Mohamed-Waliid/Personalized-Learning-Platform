
using Graduation_Project.ViewModels.Course;

namespace Graduation_Project.ViewModels
{
    public class HomeViewModel
    {
        public int StudentCount { get; set; }
        public int CourseCount { get; set; }
        public int TrackCount { get; set; }
        public IEnumerable<ShowAllCourseViewModel> courses { get; set; }
    }
}
