using Graduation_Project.Models;
using Graduation_Project.ViewModels.Course;
using System.ComponentModel.DataAnnotations;


namespace Graduation_Project.ViewModels
{
    public class RecommendationViewModel
    {
        public int ID { get; set; }

        public string? FirstName { get; set; }

        public Models.Track? Track { get; set; }

        public List<ShowAllCourseViewModel> Courses { get; set; }

        public bool err { get; set; }

        [Required(ErrorMessage = "Please enter your skills/interests separated by a comma below.")]
        [RegularExpression(@"^\s*[A-Za-z]+(?:\s+[A-Za-z]+)*(?:\s*,\s*[A-Za-z]+(?:\s+[A-Za-z]+)*)*\s*,?\s*$", ErrorMessage = "Please enter your skills as alphabetic characters and separate every two skills with a \", \"")]
        public string Skills { get; set; }

        public char? FeedBack { get; set; }
    }
}