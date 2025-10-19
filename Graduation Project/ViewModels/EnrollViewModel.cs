using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Graduation_Project.Models;


namespace Graduation_Project.ViewModels
{
    public class EnrollViewModel
    {
        [Required]
        [Display(Name = "Student Email")]
        [DataType(DataType.EmailAddress)]
        public string StudentEmail { get; set; }


        //[Required]
        //[ForeignKey("Course")]
        //[Display(Name = "Course ID")]
        //public IEnumerable<Course> CourseTracks { get; set; } = new List<Course>();
    }
}
