using Graduation_Project.ViewModels;
using Graduation_Project.ViewModels.Material;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Graduation_Project.Controllers
{
    public class MaterialController : Controller
    {
        ICourseRepo courseRepo;
        ILearningMaterialRepo repo;
        ICourseMaterialRepo cmrepo;
        IEnrollmentRepo enrepo;
        ICompletedMaterialRepo cmprepo;
        UserManager<ApplicationUser> _userManager;

        public MaterialController(ILearningMaterialRepo repo, ICourseRepo courseRepo, UserManager<ApplicationUser> userManager
            , IEnrollmentRepo enrollmentRepo, ICourseMaterialRepo cmr, ICompletedMaterialRepo cmpr)
        {
            cmrepo = cmr;
            cmprepo = cmpr;
            this.repo = repo;
            this.courseRepo = courseRepo;
            this._userManager = userManager;
            this.enrepo = enrollmentRepo;
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(int CourseID)
        {
            AddLearningMaterialViewModel obj = new AddLearningMaterialViewModel();
            obj.CourseID = CourseID;
            obj.course = await courseRepo.GetByIdAsync(CourseID);
            return View("Add", obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveAdd(AddLearningMaterialViewModel obj)
        {
            obj.course = await courseRepo.GetByIdAsync(obj.CourseID);

            if (ModelState.IsValid)
            {
                LearningMaterial mat = new LearningMaterial()
                {
                    Title = obj.Title,
                    Type = obj.Type,
                    Url = obj.Url
                };

                await repo.CreateAsync(mat);

                CourseMaterial newMaterial = new CourseMaterial()
                {
                    CourseID = obj.course.ID,
                    MaterialID = mat.ID
                };

                await cmrepo.CreateAsync(newMaterial);
                return RedirectToAction("Details", "Course", new { obj.CourseID });
            }
            
            return View("Add", obj);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int ID)
        {
            LearningMaterial mat = await repo.GetByIdAsync(ID);
            Course course = await cmrepo.GetCourseByMatID(ID);

            if (mat == null)
            {
                return NotFound();
            }

            AddLearningMaterialViewModel obj = new AddLearningMaterialViewModel()
            {
                ID = ID,
                CourseID = course.ID,
                Title = mat.Title,
                Url = mat.Url,
                Type = mat.Type,
                course = course
            };

            return View("Edit", obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveEdit(AddLearningMaterialViewModel obj)
        {
            obj.course = await courseRepo.GetByIdAsync(obj.CourseID);

            LearningMaterial mat = await repo.GetByIdAsync(obj.ID);
            mat.Title = obj.Title;
            mat.Type = obj.Type;
            mat.Url = obj.Url;

            await repo.UpdateAsync(mat);
            return RedirectToAction("Details", "Course", new { obj.CourseID });
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int ID)
        {
            LearningMaterial mat = await repo.GetByIdAsync(ID);
            Course course = await cmrepo.GetCourseByMatID(ID);
            ViewBag.course = course;

            if (mat == null)
            {
                return NotFound();
            }

            return View("Delete", mat);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveDelete(int ID)
        {
            LearningMaterial mat = await repo.GetByIdAsync(ID);
            Course course = await cmrepo.GetCourseByMatID(ID);
            await repo.DeleteAsync(mat);
            return RedirectToAction("Details", "Course", new { CourseID = course.ID });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkComplete(int CourseID, int MaterialID)
        {
            var user = await _userManager.GetUserAsync(User);
            Enrollment? enr = await enrepo.GetByCourseIDAndUserIDAsync(CourseID, user.Id);

            if (enr == null)
            {
                return NotFound();
            }

            CompletedMaterial cmp = new CompletedMaterial()
            {
                EnrollmentID = enr.ID,
                MaterialID = MaterialID
            };

            await cmprepo.CreateAsync(cmp);
            return RedirectToAction("Details", "Course", new { CourseID });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkUnComplete(int CourseID, int MaterialID)
        {
            var user = await _userManager.GetUserAsync(User);
            Enrollment? enr = await enrepo.GetByCourseIDAndUserIDAsync(CourseID, user.Id);
            CompletedMaterial? cmp = await cmprepo.GetByEnrollmentIDAndLMID(enr.ID, MaterialID);

            if (enr == null || cmp == null)
            {
                return NotFound();
            }

            await cmprepo.DeleteAsync(cmp);
            return RedirectToAction("Details", "Course", new { CourseID });
        }
    }
}