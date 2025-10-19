using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.ViewModels.Course
{
    public class ShowAllCourseViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        [Display(Name = "Difficulty Level")]
        public string DifficultyLevel { get; set; }


        public int EnrolledCount { get; set; }


        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; }

        public string Duration { get; set; }

        public string ImagePath { get; set; }

    }
}
