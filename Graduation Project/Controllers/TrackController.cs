using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Graduation_Project.ViewModels.Track;


namespace Graduation_Project.Controllers
{
    public class TrackController : Controller
    {
        private readonly ITrackRepo _trackRepo;
        private readonly IEnrollmentRepo _enRepo;

        public TrackController(ITrackRepo trackRepo, IEnrollmentRepo enRepo)
        {
            _trackRepo = trackRepo;
            _enRepo = enRepo;
        }

        public async Task<IActionResult> ShowAll()
        {
            List<ShowAllTrackViewModel> obj = new List<ShowAllTrackViewModel>();

            foreach (var track in await _trackRepo.GetAllWithCoursesAsync())
            {
                int sCount = 0;
                foreach (var course in track.CourseTracks)
                {
                    sCount += (await _enRepo.GetByCourseIDAsync(course.ID)).Count();
                }

                obj.Add(new ShowAllTrackViewModel
                {
                    ID = track.ID,
                    Name = track.Name,
                    Description = track.Description,
                    ImageURL = track.ImageURL,
                    StudentCount = sCount,
                    CourseCount = track.CourseTracks.Count()
                });
            }

            return View("ShowAll", obj);
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveAdd(TrackViewModel viewModel)
        {
            var trackExists = await _trackRepo.GetByNameAsync(viewModel.Name);

            if (trackExists != null)
            {
                ModelState.AddModelError("Name", "A track with this title already exists.");
                return View("Add", viewModel);
            }

            // Handle image saving
            string uniqueFileName = null;

            if (viewModel.TrackImageFile != null && viewModel.TrackImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tracks");

                Directory.CreateDirectory(uploadsFolder); // ensure the folder exists

                var fileExtension = Path.GetExtension(viewModel.TrackImageFile.FileName);

                uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.TrackImageFile.CopyToAsync(stream);
                }
            }

            Track track = new Track()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                ImageURL = $"/images/tracks/{uniqueFileName}" // Save path relative to wwwroot
            };

            await _trackRepo.CreateAsync(track);
            return RedirectToAction("ShowAll");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Track track = await _trackRepo.GetByIdAsync(id);
            TrackViewModel viewModel = new TrackViewModel()
            {
                ID = track.ID,
                Name = track.Name,
                Description = track.Description,
                ImageURL = track.ImageURL,
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(TrackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var track = await _trackRepo.GetByIdAsync(viewModel.ID);

                track.Name = viewModel.Name;
                track.Description = viewModel.Description;

                if (viewModel.TrackImageFile != null && viewModel.TrackImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tracks");

                    Directory.CreateDirectory(uploadsFolder); // ensure folder exists

                    var fileExtension = Path.GetExtension(viewModel.TrackImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.TrackImageFile.CopyToAsync(stream);
                    }

                    // Optional: Delete the old image file if needed
                    if (!string.IsNullOrEmpty(track.ImageURL))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", track.ImageURL.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    track.ImageURL = $"/images/tracks/{uniqueFileName}";
                }
                else
                {
                    // ⚠️ Ensure the ImageURL is preserved if no new image is uploaded
                    track.ImageURL = viewModel.ImageURL;
                }

                await _trackRepo.UpdateAsync(track);
                return RedirectToAction("ShowAll");
            }

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var track = await _trackRepo.GetByIdWithCoursesAsync(Id);
            TrackViewModel obj = new TrackViewModel()
            {
                ID = Id,
                Name = track.Name,
                Description = track.Description,
                ImageURL = track.ImageURL
            };

            return View("Delete", obj);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDelete(int ID)
        {
            var track = await _trackRepo.GetByIdAsync(ID);
            await _trackRepo.DeleteAsync(track);
            return RedirectToAction("ShowAll");
        }

        public async Task<IActionResult> Courses(int ID)
        {
            List<Course> courses = await _trackRepo.GetCoursesAsync(ID);
            return View("ShowByTrackID", courses);
        }
    }
}