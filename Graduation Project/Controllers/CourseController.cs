using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.ViewModels.Course;
using Graduation_Project.ViewModels.Material;
using Graduation_Project.ViewModels.Project;
using Graduation_Project.ViewModels.Quiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Graduation_Project.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDBContext ctx;

        private readonly IQuizRepo _quizRepo;
        private readonly ITrackRepo _trackRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly IEnrollmentRepo _enrollmentRepo;
        private readonly ICourseTrackRepo _courseTrackRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILearningMaterialRepo _learningMaterialRepo;


        public CourseController(ApplicationDBContext ct, ICourseRepo courseRepo, IEnrollmentRepo enrollmentRepo, ITrackRepo trackRepo, ICourseTrackRepo courseTrackRepo,
            UserManager<ApplicationUser> userManager, ILearningMaterialRepo learningMaterialRepo, IProjectRepo projectRepo, IQuizRepo quizRepo)
        {
            ctx = ct;
            _quizRepo = quizRepo;
            _trackRepo = trackRepo;
            _courseRepo = courseRepo;
            _projectRepo = projectRepo;
            _userManager = userManager;
            _enrollmentRepo = enrollmentRepo;
            _courseTrackRepo = courseTrackRepo;
            _learningMaterialRepo = learningMaterialRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> ShowAll()
        {
            var obj = new List<ShowAllCourseViewModel>();

            List<Course> Courses = await ctx.Courses
                .AsNoTracking()
                .Include(c => c.Instructor)
                .ToListAsync();

            foreach (var course in Courses)
            {
                obj.Add(new ShowAllCourseViewModel
                {
                    ID = course.ID,
                    Title = course.Title,
                    Duration = course.Duration,
                    ImagePath = course.ImagePath,
                    Description = course.Description,
                    InstructorName = course.Instructor.FullName,
                    DifficultyLevel = course.DifficultyLevel,
                    EnrolledCount = ctx.Enrollments
                        .AsNoTracking()
                        .Where(e => e.CourseID == course.ID)
                        .Count()
                }
                );
            }

            return View("ShowAll", obj);
        }

        public async Task<IActionResult> ShowByTrackID(int TrackID)
        {
            var obj = new List<ShowAllCourseViewModel>();

            foreach (var course in await _courseRepo.GetByTrackIdAsync(TrackID))
            {
                var Instructor = await _userManager.FindByIdAsync(course.InstructorID);


                obj.Add(new ShowAllCourseViewModel
                {
                    ID = course.ID,
                    Title = course.Title,
                    Duration = course.Duration,
                    ImagePath = course.ImagePath,
                    Description = course.Description,
                    InstructorName = Instructor.FullName,
                    DifficultyLevel = course.DifficultyLevel,
                    EnrolledCount = course.Enrollments.Count
                }
                );
            }
            var track = await _trackRepo.GetByIdAsync(TrackID);
            ViewBag.trackname = track.Name;
            return View("ShowByTrackID", obj);
        }

        [Authorize]
        public async Task<IActionResult> Details(int CourseID)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["Error"] = "User not found. Please log in.";
                return RedirectToAction("Login", "Account");
            }

            var course = await _courseRepo.GetByIdWithDetailsAsync(CourseID);
            if (course == null)
            {
                return NotFound();
            }

            var userId = user.Id;
            var isEnrolled = User.IsInRole("Student") && await _enrollmentRepo.IsStudentEnrolledAsync(userId, course.ID);

            var materials = await _learningMaterialRepo.GetByCourseIdWithCompletionsAsync(course.ID);
            var quizzes = await _quizRepo.GetByCourseIdAsync(course.ID);
            var projects = await _projectRepo.GetByCourseIdAsync(course.ID);

            var quizIds = quizzes.Select(q => q.ID).ToList();
            var completedQuizIds = isEnrolled ? await _quizRepo.GetCompletedQuizIdsAsync(quizIds, userId) : new List<int>();

            var model = new CourseDetailsViewModel
            {
                Id = course.ID,
                Title = course.Title,
                Description = course.Description,
                ImagePath = course.ImagePath,
                Instructor = course.Instructor,
                isOwner = course.Instructor.Id == user.Id,
                Duration = course.Duration,
                DifficultyLevel = course.DifficultyLevel,
                EnrolledCount = course.Enrollments?.Count ?? 0,
                Tracks = course.CourseTracks?.Select(ct => ct.Track.Name).ToList() ?? new List<string>(),
                IsEnrolled = isEnrolled,
                Progress = isEnrolled ? await CalculateProgressAsync(course.ID, userId) : 0,
                Materials = materials.Select(m => new MaterialViewModel
                {
                    Id = m.ID,
                    Title = m.Title,
                    Type = m.Type,
                    Url = m.Url,
                    IsCompleted = isEnrolled && (m.CompletedMaterials?.Any(cm => cm.Enrollment?.Student?.Id == userId) ?? false)
                }).ToList(),
                Quizzes = quizzes.Select(q => new QuizViewModel
                {
                    Id = q.ID,
                    Title = q.Title,
                    DifficultyLevel = q.DifficultyLevel,
                    Status = isEnrolled ? (completedQuizIds.Contains(q.ID) ? "Completed" : "Not Started") : "Locked"
                }).ToList(),
                Projects = projects.Select(p => new ProjectViewModel
                {
                    Id = p.ID,
                    Title = p.Title,
                    Description = p.Description,
                    DifficultyLevel = p.DifficultyLevel,
                    Status = isEnrolled ? "Available" : "Locked"
                }).ToList()
            };

            return View(model);
        }

        private async Task<double> CalculateProgressAsync(int courseId, string userId)
        {
            var materials = await _learningMaterialRepo.GetByCourseIdWithCompletionsAsync(courseId);
            var completedCount = materials.Count(m => m.CompletedMaterials?.Any(cm => cm.Enrollment?.Student?.Id == userId) ?? false);
            var totalCount = materials.Count;
            return totalCount > 0 ? (double)completedCount / totalCount * 100.0 : 0;
        }

        public async Task<IActionResult> Add()
        {
            var viewModel = new CourseFormViewModel()
            {
                AvailableTracks = await _trackRepo.GetAllAsync()
            };
            return View("Add", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveAdd(CourseFormViewModel viewModel)
        {
            var allTracks = await _trackRepo.GetAllAsync();

            if (ModelState.IsValid)
            {
                var courseExist = await _courseRepo.GetByNameAsync(viewModel.Title, "Title");

                if (courseExist != null)
                {
                    ModelState.AddModelError("Title", "A course with this title already exists.");
                    viewModel.AvailableTracks = allTracks;
                    return View("Add", viewModel);
                }

                var user = await _userManager.GetUserAsync(User);

                // Handle image saving
                string uniqueFileName = null;

                if (viewModel.CourseImageFile != null && viewModel.CourseImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");

                    Directory.CreateDirectory(uploadsFolder); // ensure the folder exists

                    var fileExtension = Path.GetExtension(viewModel.CourseImageFile.FileName);

                    uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.CourseImageFile.CopyToAsync(stream);
                    }
                }

                Course course = new Course()
                {
                    InstructorID = user.Id,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    DifficultyLevel = viewModel.DifficultyLevel,
                    Duration = viewModel.Duration,
                    ImagePath = $"/images/courses/{uniqueFileName}" // Save path relative to wwwroot
                };

                await _courseRepo.CreateAsync(course);

                foreach (var id in viewModel.SelectedTrackIds)
                {
                    CourseTrack courseTrack = new CourseTrack()
                    {
                        TrackID = id,
                        CourseID = course.ID
                    };

                    await _courseTrackRepo.CreateAsync(courseTrack);
                }

                viewModel.AvailableTracks = allTracks;
                return RedirectToAction("Index", "Dashboard");
            }

            viewModel.AvailableTracks = allTracks;
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            Course course = await _courseRepo.GetByIdAsync(Id);

            var viewModel = new CourseFormViewModel()
            {
                ID = Id,
                Title = course.Title,
                Duration = course.Duration,
                ImageURL = course.ImagePath,
                Description = course.Description,
                DifficultyLevel = course.DifficultyLevel,
                AvailableTracks = await _trackRepo.GetAllAsync(),
                SelectedTrackIds = course.CourseTracks?.Select(t => t.TrackID).ToList() ?? new List<int>()
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveEdit(CourseFormViewModel viewModel)
        {
            var allTracks = await _trackRepo.GetAllAsync();

            if (ModelState.IsValid)
            {
                Course courseExist = await _courseRepo.GetByNameAsync(viewModel.Title, "Title");

                if (courseExist != null && courseExist.ID != viewModel.ID)
                {
                    ModelState.AddModelError("Title", "A course with this title already exists.");
                    viewModel.AvailableTracks = allTracks;
                    return View("Edit", viewModel);
                }

                Course course = await _courseRepo.GetByIdAsync(viewModel.ID);

                if (viewModel.CourseImageFile != null && viewModel.CourseImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileExtension = Path.GetExtension(viewModel.CourseImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.CourseImageFile.CopyToAsync(stream);
                    }

                    // Optionally delete old image
                    if (!string.IsNullOrEmpty(course.ImagePath))
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", course.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    course.ImagePath = $"/images/courses/{uniqueFileName}";
                }

                course.Title = viewModel.Title;
                course.Duration = viewModel.Duration;
                course.Description = viewModel.Description;
                course.DifficultyLevel = viewModel.DifficultyLevel;

                await _courseTrackRepo.RemoveByCourseIdAsync(course.ID);

                foreach (var id in viewModel.SelectedTrackIds)
                {
                    CourseTrack courseTrack = new CourseTrack()
                    {
                        TrackID = id,
                        CourseID = course.ID
                    };
                    await _courseTrackRepo.CreateAsync(courseTrack);
                }

                await _courseRepo.UpdateAsync(course);
                return RedirectToAction("Index", "Dashboard");

            }

            viewModel.AvailableTracks = allTracks;
            return View("Edit", viewModel);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int ID)
        {
            return View("Delete", await _courseRepo.GetByIdAsync(ID));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveDelete(int ID)
        {
            Course course = await _courseRepo.GetByIdAsync(ID);
            await _courseRepo.DeleteAsync(course);
            await _courseTrackRepo.RemoveByCourseIdAsync(ID);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}