using Graduation_Project.Data;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Repositories.Interfaces;

namespace Graduation_Project.Repositories
{
    public class QuizRepo : Repository<Quiz>, IQuizRepo
    {
        ApplicationDBContext context;

        public QuizRepo(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Quiz?> GetByIDWithCourse(int ID)
        {
            return await context.Quizzes.Include(q => q.Course).FirstOrDefaultAsync(q => q.ID == ID);
        }

        public async Task<List<Quiz>> GetByCourseIdAsync(int courseId)
        {
            return await context.Quizzes.Where(q => q.CourseID == courseId).ToListAsync();
        }

        public async Task<bool> HasResultAsync(int quizId, string studentId)
        {
            return await _context.QuizResults
                .AnyAsync(qr => qr.QuizID == quizId && qr.StudentID == studentId);
        }

        public async Task<List<int>> GetCompletedQuizIdsAsync(List<int> quizIds, string studentId)
        {
            return await _context.QuizResults
                .Where(qr => quizIds.Contains(qr.QuizID) && qr.StudentID == studentId)
                .Select(qr => qr.QuizID)
                .ToListAsync();
        }
    }
}
