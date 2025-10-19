using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.ViewModels;
using Graduation_Project.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Graduation_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly ICompletedMaterialRepo compRepo;
        private readonly ICourseMaterialRepo cmRepo;
        private readonly IEnrollmentRepo enRepo;
        private readonly IRecommendationRepo recRepo;
        private readonly ICourseRepo courseRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> roleManager;

        public DashboardController(ApplicationDBContext ct, RoleManager<IdentityRole> rm, UserManager<ApplicationUser> userManager, IEnrollmentRepo e, ICourseRepo cr, ICompletedMaterialRepo cmpr, ICourseMaterialRepo cmr, IRecommendationRepo rr)
        {
            context = ct;
            roleManager = rm;
            recRepo = rr;
            compRepo = cmpr;
            cmRepo = cmr;
            enRepo = e;
            courseRepo = cr;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                ShowAllViewModel model = new ShowAllViewModel();
                string? s_role_id = await context.Roles
                    .Where(r => r.Name == "Student")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                string? i_role_id = await context.Roles
                    .Where(r => r.Name == "Instructor")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                string? a_role_id = await context.Roles
                    .Where(r => r.Name == "Admin")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

                model.Roles = await context.Roles.ToListAsync();

                model.Students = await context.Users
                    .AsNoTracking()
                    .Where(u => context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == s_role_id)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                    )
                    .Where(u => !context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == a_role_id)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                    )
                    .ToListAsync();

                model.Instructors = await context.Users
                    .AsNoTracking()
                    .Where(u => context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == i_role_id)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                    ).Where(u => !context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == a_role_id)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                    )
                    .ToListAsync();

                model.Admins = await context.Users
                    .AsNoTracking()
                    .Where(u => context.UserRoles
                        .AsNoTracking()
                        .Where(ur => ur.RoleId == a_role_id)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id)
                    )
                    .ToListAsync();

                return View("AdminIndex", model);
            }

            var user = await _userManager.GetUserAsync(User);
            DashboardViewModel obj = new DashboardViewModel();
            if (User.IsInRole("Student"))
            {
                List<Enrollment>? enrollments = await enRepo.GetByStudentIDAsync(user.Id);
                obj.LastRecommendation = await recRepo.GetLastRecommendationAsync(user.Id);

                if (enrollments != null && enrollments.Count != 0)
                {
                    obj.CoursesProgress = new List<Tuple<ShowAllCourseViewModel, int>>();

                    var enrollmentIds = new List<int>();
                    foreach (var enr in enrollments)
                    {
                        Course? course = await courseRepo.GetWithEnrollments(enr.CourseID);
                        if (course == null) continue;

                        List<LearningMaterial>? materials = await cmRepo.GetByCourseIDAsync(course.ID);
                        int total = materials != null ? materials.Count : 0;
                        int completed = 0;

                        if (materials != null)
                        {
                            foreach (var material in materials)
                            {
                                completed += (await compRepo.CheckIfCompletedAsync(enr.ID, material.ID)) ? 1 : 0;
                            }
                        }

                        ShowAllCourseViewModel ViewModel = new ShowAllCourseViewModel()
                        {
                            ID = course.ID,
                            Title = course.Title,
                            Duration = course.Duration,
                            ImagePath = course.ImagePath,
                            Description = course.Description,
                            InstructorName = course.Instructor?.FullName ?? "Unknown",
                            DifficultyLevel = course.DifficultyLevel,
                            EnrolledCount = course.Enrollments?.Count ?? 0
                        };

                        int percentage = total > 0 ? (int)Math.Round((float)completed / total * 100) : 0;
                        obj.CoursesProgress.Add(new Tuple<ShowAllCourseViewModel, int>(ViewModel, percentage));
                        enrollmentIds.Add(enr.ID);
                    }
                    ViewBag.EnrollmentIds = enrollmentIds;
                }

                return View("StudentIndex", obj);
            }

            List<Course> Courses = await courseRepo.GetByInstructorIDAsync(user.Id, true);

            obj.Courses = new List<ShowAllCourseViewModel>();
            if (Courses != null && Courses.Count != 0)
            {
                foreach (var course in Courses)
                {
                    obj.Courses.Add(new ShowAllCourseViewModel()
                    {
                        ID = course.ID,
                        Title = course.Title,
                        Duration = course.Duration,
                        ImagePath = course.ImagePath,
                        Description = course.Description,
                        InstructorName = course.Instructor?.FullName ?? "Unknown",
                        DifficultyLevel = course.DifficultyLevel,
                        EnrolledCount = course.Enrollments?.Count ?? 0
                    });
                }
            }

            return View("InstructorIndex", obj);
        }
    }
}