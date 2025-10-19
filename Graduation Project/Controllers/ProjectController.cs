using Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.ViewModels.Project;


namespace Graduation_Project.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepo repo;
        private readonly ICourseRepo courseRepo;
        private readonly IEnrollmentRepo enrollmentRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(IProjectRepo repo, ICourseRepo courseRepo, UserManager<ApplicationUser> userManager, IEnrollmentRepo enrollmentRepo)
        {
            this.repo = repo;
            this.courseRepo = courseRepo;
            this.userManager = userManager;
            this.enrollmentRepo = enrollmentRepo;
        }

        public async Task<IActionResult> ShowAll(int CourseID)
        {
            List<Project> projects = await repo.GetByCourseIdAsync(CourseID);
            ViewBag.CourseName = (await courseRepo.GetByIdAsync(CourseID))?.Title ?? "Unknown";
            return View("ShowAll", projects);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(int CourseID)
        {
            AddProjectViewModel obj = new AddProjectViewModel();
            obj.CourseID = CourseID;
            return View("Add", obj);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdd(AddProjectViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Project project = new Project()
                {
                    Title = obj.Title,
                    Description = obj.Description,
                    DifficultyLevel = obj.DifficultyLevel,
                    CourseID = obj.CourseID
                };

                await repo.CreateAsync(project);
                return RedirectToAction("Details", "Course", new { obj.CourseID });
            }

            return View("Add", obj);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int ID)
        {
            Project project = await repo.GetByIdAsync(ID);
            if (project == null)
            {
                return NotFound();
            }
            AddProjectViewModel obj = new AddProjectViewModel()
            {
                ID = project.ID,
                Title = project.Title,
                DifficultyLevel = project.DifficultyLevel,
                Description = project.Description,
                CourseID = project.CourseID
            };

            return View("Edit", obj);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(AddProjectViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Project project = await repo.GetByIdAsync(obj.ID);
                if (project == null)
                {
                    return NotFound();
                }
                project.Title = obj.Title;
                project.Description = obj.Description;
                project.DifficultyLevel = obj.DifficultyLevel;
                await repo.UpdateAsync(project);
                return RedirectToAction("Details", "Course", new { obj.CourseID });
            }

            return View("Edit", obj);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int ID)
        {
            Project? project = await repo.GetByIdWithCourseAsync(ID);
            if (project == null)
            {
                return NotFound();
            }
            return View("Delete", project);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveDelete(int ID)
        {
            Project obj = await repo.GetByIdAsync(ID);
            if (obj == null)
            {
                return NotFound();
            }
            await repo.DeleteAsync(obj);
            return RedirectToAction("Details", "Course", new { obj.CourseID });
        }
    }
}