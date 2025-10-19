using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class QuestionRepo : Repository<Question>, IQuestionRepo
    {
        ApplicationDBContext _context;
        IQuizRepo quizRepo;
        public QuestionRepo(ApplicationDBContext context, IQuizRepo quizRepo) : base(context)
        {
            _context = context;
            this.quizRepo = quizRepo;
        }

        public async Task<List<Question>> GetByQuizIDAsync(int QuizID)
        {
            return _context.Questions.Where(q => q.QuizID == QuizID).ToList();
        }
    }
}
