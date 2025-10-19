using System.Text.Json;
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
    public class RecommendationController : Controller
    {
        private readonly ApplicationDBContext ctx;
        private readonly HttpClient _httpClient;

        private readonly ITrackRepo _trackRepo;
        private readonly IRecommendationRepo _recRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecommendationController(ApplicationDBContext context, ITrackRepo trackRepo, IRecommendationRepo recRepo, UserManager<ApplicationUser> userManager)
        {
            ctx = context;
            _httpClient = new HttpClient();
            _trackRepo = trackRepo;
            _recRepo = recRepo;
            _userManager = userManager;
        }

        private List<ShowAllCourseViewModel> GetRecommendedCourses(int TrackID)
        {
            List<ShowAllCourseViewModel> obj = new();

            Course? Beginner = ctx.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .Where(c => c.DifficultyLevel == "Beginner")
                .Where(c => ctx.CourseTracks
                    .AsNoTracking()
                    .Where(ct => ct.TrackID == TrackID)
                    .Where(ct => ct.CourseID == c.ID)
                    .Count() != 0
                )
                .OrderByDescending(c => c.Enrollments.Count)
                .FirstOrDefault();

            Course? Intermediate = ctx.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .Where(c => c.DifficultyLevel == "Intermediate")
                .Where(c => ctx.CourseTracks
                    .AsNoTracking()
                    .Where(ct => ct.TrackID == TrackID)
                    .Where(ct => ct.CourseID == c.ID)
                    .Count() != 0
                )
                .OrderByDescending(c => c.Enrollments.Count)
                .FirstOrDefault();

            Course? Advanced = ctx.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .Where(c => c.DifficultyLevel == "Advanced")
                .Where(c => ctx.CourseTracks
                    .AsNoTracking()
                    .Where(ct => ct.TrackID == TrackID)
                    .Where(ct => ct.CourseID == c.ID)
                    .Count() != 0
                )
                .OrderByDescending(c => c.Enrollments.Count)
                .FirstOrDefault();

            if (Beginner != null)
            {
                obj.Add(new ShowAllCourseViewModel
                {
                    ID = Beginner.ID,
                    Title = Beginner.Title,
                    Duration = Beginner.Duration,
                    ImagePath = Beginner.ImagePath,
                    Description = Beginner.Description,
                    InstructorName = Beginner.Instructor.FullName,
                    DifficultyLevel = Beginner.DifficultyLevel,
                    EnrolledCount = Beginner.Enrollments.Count
                });
            }

            if (Intermediate != null)
            {
                obj.Add(new ShowAllCourseViewModel
                {
                    ID = Intermediate.ID,
                    Title = Intermediate.Title,
                    Duration = Intermediate.Duration,
                    ImagePath = Intermediate.ImagePath,
                    Description = Intermediate.Description,
                    InstructorName = Intermediate.Instructor.FullName,
                    DifficultyLevel = Intermediate.DifficultyLevel,
                    EnrolledCount = Intermediate.Enrollments.Count
                });
            }

            if (Advanced != null)
            {
                obj.Add(new ShowAllCourseViewModel
                {
                    ID = Advanced.ID,
                    Title = Advanced.Title,
                    Duration = Advanced.Duration,
                    ImagePath = Advanced.ImagePath,
                    Description = Advanced.Description,
                    InstructorName = Advanced.Instructor.FullName,
                    DifficultyLevel = Advanced.DifficultyLevel,
                    EnrolledCount = Advanced.Enrollments.Count
                });
            }

            return obj;
        }

        public IActionResult Index()
        {
            RecommendationViewModel obj = new RecommendationViewModel();
            return View("Index", obj);
        }

        [Authorize]
        public async Task<IActionResult> CreateRecommendation(RecommendationViewModel obj)
        {
            if (obj.Skills == null)
            {
                return NotFound();
            }

            obj.Skills.Replace(' ', '$');
            try
            {
                // Step 1: Fetch data from the Random User API
                var apiUrl = "http://127.0.0.1:5000/" + obj.Skills;
                var response = await _httpClient.GetAsync(apiUrl);

                // Step 2: Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Step 3: Read and parse the JSON response
                var responseData = await response.Content.ReadAsStringAsync();

                using JsonDocument jsonDocument = JsonDocument.Parse(responseData);
                JsonElement root = jsonDocument.RootElement;

                var user = await _userManager.GetUserAsync(User);

                obj.Track = await _trackRepo.GetByNameAsync(root.GetProperty("track").GetString());
                obj.err = false;

                if (obj.Track != null)
                {
                    Recommendation recommendation = new Recommendation
                    {
                        TrackID = obj.Track.ID,
                        StudentID = user.Id,
                        Skills = obj.Skills
                    };

                    await _recRepo.CreateAsync(recommendation);
                    obj.ID = recommendation.ID;
                    obj.Courses = GetRecommendedCourses(obj.Track.ID);
                }
                else
                {
                    obj.err = true;
                }

                return View("Index", obj);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error fetching data from API: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFeedBack(RecommendationViewModel obj)
        {
            obj.Track = await _trackRepo.GetByIdAsync(obj.Track.ID);
            obj.Courses = GetRecommendedCourses(obj.Track.ID);
            Recommendation rec = await _recRepo.GetByIdAsync(obj.ID);
            rec.Feedback = obj.FeedBack - '0';
            await _recRepo.UpdateAsync(rec);
            return View("Index", obj);
        }
    }
}