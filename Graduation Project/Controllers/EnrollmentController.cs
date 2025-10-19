using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Graduation_Project.Controllers
{
    public class EnrollmentController : Controller
    {
        readonly ApplicationDBContext _context;
        readonly IEnrollmentRepo _enrollmentRepo;
        readonly UserManager<ApplicationUser> _userManager;
        readonly ICompletedMaterialRepo _completedMaterialRepo;

        public EnrollmentController(IEnrollmentRepo enrollmentRepo, ApplicationDBContext context , UserManager<ApplicationUser> userManager, ICompletedMaterialRepo completedMaterialRepo)
        {
            _context = context;
            _userManager = userManager;
            _enrollmentRepo = enrollmentRepo;
            _completedMaterialRepo = completedMaterialRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var student = await _userManager.GetUserAsync(User);

            var success = await _enrollmentRepo.EnrollStudentAsync(student.Id, courseId);

            if (!success)
            {
                TempData["Error"] = "You are already enrolled in this course.";
                return RedirectToAction("Details", "Course", new { CourseID = courseId });
            }

            TempData["Success"] = "Successfully enrolled in the course!";
            return RedirectToAction("Details", "Course", new { CourseID = courseId });
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DisplayEnrolledCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var enrollments = await _enrollmentRepo.GetEnrollmentsWithCoursesAndMaterialsAsync(user.Id);
            var enrolledCourses = new List<EnrolledCoursesViewModel>();

            foreach (var enrollment in enrollments)
            {
                var course = enrollment.Course;
                var completedLessons = enrollment.CompletedMaterials?.Count() ?? 0;
                var totalLessons = course.CourseMaterials?.Count ?? 0;

                enrolledCourses.Add(new EnrolledCoursesViewModel
                {
                    CourseId = course.ID,
                    Title = course.Title,
                    ImageUrl = course.ImagePath,
                    DifficultyLevel = course.DifficultyLevel,
                    Duration = course.Duration,
                    Progress = totalLessons > 0 ? (double)completedLessons / totalLessons * 100.0 : 0
                });
            }

            return View("DisplayEnrolledCourses", enrolledCourses);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _enrollmentRepo.GetByIdAsync(id);
            var student = await _userManager.GetUserAsync(User);

            if (enrollment != null)
            {
                await _completedMaterialRepo.DeleteByEnrollmentIdAsync(enrollment.ID);
                var quizResults = _context.QuizResults.Where(q => q.StudentID == student.Id);
                _context.QuizResults.RemoveRange(quizResults);
                await _context.SaveChangesAsync();
                await _enrollmentRepo.DeleteAsync(enrollment);
            }

            TempData["Message"] = "Enrollment deleted successfully.";

            return RedirectToAction("Index", "Dashboard");
        }
    }
}