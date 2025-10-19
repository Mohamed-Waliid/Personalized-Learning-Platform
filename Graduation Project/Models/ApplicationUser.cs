using Graduation_Project.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public string? ProfilePicture { get; set; }


        // Navigation Properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
    }
}