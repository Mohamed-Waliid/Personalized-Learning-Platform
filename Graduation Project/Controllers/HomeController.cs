using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.ViewModels;
using Graduation_Project.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Data;


namespace Graduation_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext ctx;
        private readonly ILogger<HomeController> _logger;
        private readonly ICourseRepo _courseRepo;
        private readonly ITrackRepo _trepo;

        public HomeController(ApplicationDBContext ct, ILogger<HomeController> logger, ICourseRepo courseRepo, ITrackRepo tr)
        {
            ctx = ct;
            _trepo = tr;
            _logger = logger;
            _courseRepo = courseRepo;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseRepo.GetWithEnrollmentsAsync(20);

            var viewModelCourses = courses.Select(course => new ShowAllCourseViewModel
            {
                ID = course.ID,
                Title = course.Title,
                Description = course.Description,
                ImagePath = course.ImagePath,
                InstructorName = course.Instructor.FullName,
                Duration = course.Duration,
                DifficultyLevel = course.DifficultyLevel,
                EnrolledCount = course.Enrollments?.Count ?? 0
            }).ToList();

            int TrackCnt = (await _trepo.GetAllAsync()).Count();

            HomeViewModel obj = new HomeViewModel()
            {
                courses = viewModelCourses,
                CourseCount = ctx.Courses.Count(),
                TrackCount = TrackCnt
            };

            string? s_role_id = ctx.Roles
                .AsNoTracking()
                .Where(r => r.Name == "Student")
                .Select(r => r.Id)
                .FirstOrDefault();

            obj.StudentCount = ctx.Users.AsNoTracking()
                .Where(u => ctx.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.RoleId == s_role_id)
                    .Select(ur => ur.UserId)
                    .Contains(u.Id)
                )
                .Count();

            // obj.StudentCount = (await _userManager.GetUsersInRoleAsync("Student")).Count();
            return View(obj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.ID ?? HttpContext.TraceIdentifier });
        }*/
    }
}