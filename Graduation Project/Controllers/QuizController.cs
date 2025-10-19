using Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.ViewModels.Quiz;
using Graduation_Project.ViewModels.Question;
using Graduation_Project.Data;
using Graduation_Project.Repositories;

namespace Graduation_Project.Controllers
{
    public class QuizController : Controller
    {
        IQuizRepo _repo;
        ICourseRepo _courseRepo;
        IQuestionRepo _questionRepo;
        ApplicationDBContext _context;
        IEnrollmentRepo _enrollmentRepo;
        UserManager<ApplicationUser> _userManager;

        public QuizController(IQuizRepo repo,ApplicationDBContext context , ICourseRepo courseRepo,IQuestionRepo questionRepo , UserManager<ApplicationUser> userManager
            , IEnrollmentRepo enrollmentRepo)
        {
            _repo = repo;
            _context = context;
            _courseRepo = courseRepo;
            _userManager = userManager;
            _questionRepo = questionRepo;
            _enrollmentRepo = enrollmentRepo;
        }


        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(int CourseID)
        {
            AddQuizViewModel model = new AddQuizViewModel();
            model.CourseID = CourseID;
            model.course = await _courseRepo.GetByIdAsync(CourseID);

            return View("Add", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveAdd(AddQuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.course = await _courseRepo.GetByIdAsync(model.CourseID);
                Quiz newQuiz = new Quiz()
                {
                    CourseID = model.CourseID,
                    Title = model.Title,
                    DifficultyLevel = model.DifficultyLevel
                };

                await _repo.CreateAsync(newQuiz);
                return RedirectToAction("Details", "Course", new { model.CourseID });
            }
            return View("Add",model);
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int ID)
        {
            Quiz quiz = await _repo.GetByIDWithCourse(ID);
            if (quiz == null)
            {
                return NotFound();
            }

            AddQuizViewModel model = new AddQuizViewModel()
            {
                ID = ID,
                CourseID = quiz.CourseID,
                Title = quiz.Title,
                DifficultyLevel = quiz.DifficultyLevel,
                course = quiz.Course
            };

            return View("Edit", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveEdit(AddQuizViewModel model)
        {
            model.course = await _courseRepo.GetByIdAsync(model.CourseID);

            Quiz quiz = await _repo.GetByIDWithCourse(model.ID);
            quiz.Title = model.Title;
            quiz.DifficultyLevel = model.DifficultyLevel;

            await _repo.UpdateAsync(quiz);
            return RedirectToAction("Details", "Course", new { model.CourseID });
        }

        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int ID)
        {
            Quiz quiz = await _repo.GetByIDWithCourse(ID);
            if (quiz == null)
            {
                return NotFound();
            }

            return View("Delete", quiz);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> SaveDelete(int ID)
        {
            Quiz quiz = await _repo.GetByIDWithCourse(ID);
            await _repo.DeleteAsync(quiz);
            return RedirectToAction("Details", "Course", new { quiz.CourseID });
        }

        public async Task<IActionResult> TakeQuiz(int QuizID , int CourseID)
        {
            var questions = await _questionRepo.GetByQuizIDAsync(QuizID);
            var list = new List<QuestionDetailsViewModel>();

            foreach (var question in questions)
            {
                var viewModel = new QuestionDetailsViewModel()
                {
                    ID = question.ID,
                    Text = question.Text,
                    AnswerOptions = question.AnswerOptions,
                };
                list.Add(viewModel);
            }

            ViewBag.QuizName = (await _repo.GetByIdAsync(QuizID)).Title;
            ViewBag.QuizID = QuizID;
            ViewBag.CourseID = CourseID;

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(int QuizID, int CourseID , List<QuestionDetailsViewModel> Questions)
        {
            var student = await _userManager.GetUserAsync(User);
            var storedQuestions = await _questionRepo.GetByQuizIDAsync(QuizID);

            int score = 0;
            var resultList = new List<QuestionDetailsViewModel>();

            foreach (var submitted in Questions)
            {
                var stored = storedQuestions.FirstOrDefault(q => q.ID == submitted.ID);
                if (stored == null) continue;

                var isCorrect = stored.CorrectAnswer.Trim().Equals(submitted.SelectedAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);

                if (isCorrect) score++;

                resultList.Add(new QuestionDetailsViewModel
                {
                    ID = stored.ID,
                    Text = stored.Text,
                    IsCorrect = isCorrect,
                    AnswerOptions = stored.AnswerOptions,
                    CorrectAnswer = stored.CorrectAnswer,
                    SelectedAnswer = submitted.SelectedAnswer
                });
            }

            // Save result
            var quizResult = new QuizResult
            {
                QuizID = QuizID,
                StudentID = student.Id,
                AttemptDate = DateTime.Now,
                Score = score
            };
            _context.QuizResults.Add(quizResult);
            await _context.SaveChangesAsync();

            ViewBag.QuizID = QuizID;
            ViewBag.QuizName = (await _repo.GetByIdAsync(QuizID))?.Title;
            ViewBag.ShowResults = true;
            ViewBag.Score = score;
            ViewBag.CourseID = CourseID;

            return View("TakeQuiz", resultList);
        }

    }
}