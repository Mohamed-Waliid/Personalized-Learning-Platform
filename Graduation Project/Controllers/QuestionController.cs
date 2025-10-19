using Graduation_Project.Repositories.Interfaces;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.Repositories;
using Graduation_Project.ViewModels.Question;

namespace Graduation_Project.Controllers
{
    public class QuestionController : Controller
    {
        IQuestionRepo _repo;
        IQuizRepo _quizRepo;
        ICourseRepo _courseRepo;
        IEnrollmentRepo _enrollmentRepo;
        UserManager<ApplicationUser> _userManager;

        public QuestionController(IQuestionRepo repo, IQuizRepo quizRepo, IEnrollmentRepo enrollmentRepo
            , UserManager<ApplicationUser> userManager, ICourseRepo courseRepo)
        {
            _repo = repo;
            _quizRepo = quizRepo;
            _courseRepo = courseRepo;
            _userManager = userManager;
            _enrollmentRepo = enrollmentRepo;
        }

        [Authorize]
        public async Task<IActionResult> ShowAll(int QuizID)
        {
            var questions = await _repo.GetByQuizIDAsync(QuizID);
            var quiz = await _quizRepo.GetByIdAsync(QuizID);

            var model = new List<QuestionDetailsViewModel>();

            foreach (var question in questions)
            {
                model.Add(new QuestionDetailsViewModel
                {
                    ID = question.ID,
                    Text = question.Text,
                    AnswerOptions = question.AnswerOptions,
                    CorrectAnswer = question.CorrectAnswer,
                    QuizID = QuizID,
                    QuizName = quiz.Title,
                });
            }

            ViewBag.QuizID = QuizID; 
            ViewBag.QuizName = quiz.Title; 
            ViewBag.CourseID = quiz.CourseID; 
            
            return View("ShowAll", model);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(int QuizID)
        {
            var model = new QuestionFormViewModel()
            {
                QuizID = QuizID
            };
            return View("Add", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdd(QuestionFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Split the options and trim spaces
                var options = viewModel.AnswerOptions
                                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(o => o.Trim())
                                       .ToList();

                // Check if the correct answer matches exactly one of the options
                int matchCount = options.Count(o => string.Equals(o, viewModel.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (matchCount != 1)
                {
                    ModelState.AddModelError("CorrectAnswer", "The correct answer must exactly match one of the answer options.");
                    return View("Add", viewModel);
                }

                Question question = new Question()
                {
                    Text = viewModel.Text,
                    AnswerOptions = viewModel.AnswerOptions,
                    CorrectAnswer = viewModel.CorrectAnswer,
                    QuizID = viewModel.QuizID
                };

                await _repo.CreateAsync(question);
                return RedirectToAction("ShowAll", new { QuizID = viewModel.QuizID });
            }

            return View("Add", viewModel);
        }


        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int QuestionID , int QuizID)
        {
            Question question = await _repo.GetByIdAsync(QuestionID);

            var model = new QuestionFormViewModel()
            {
                ID = QuestionID,
                QuizID = QuizID,
                Text = question.Text,
                AnswerOptions = question.AnswerOptions,
                CorrectAnswer = question.CorrectAnswer,
            };
            return View("Edit",model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(QuestionFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Split the options and trim spaces
                var options = model.AnswerOptions
                                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(o => o.Trim())
                                       .ToList();

                // Check if the correct answer matches exactly one of the options
                int matchCount = options.Count(o => string.Equals(o, model.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (matchCount != 1)
                {
                    ModelState.AddModelError("CorrectAnswer", "The correct answer must exactly match one of the answer options.");
                    return View("Edit", model);
                }

                var question = await _repo.GetByIdAsync(model.ID);

                question.Text = model.Text;
                question.AnswerOptions = model.AnswerOptions;
                question.CorrectAnswer = model.CorrectAnswer;
                await _repo.UpdateAsync(question);
                return RedirectToAction("ShowAll", new { QuizID = model.QuizID });
            }
            return View("Edit", model);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int QuestionID,int QuizID)
        {
            Question Question = await _repo.GetByIdAsync(QuestionID);
            await _repo.DeleteAsync(Question);
            return RedirectToAction("ShowAll", new { QuizID = QuizID });
        }
    }
}
