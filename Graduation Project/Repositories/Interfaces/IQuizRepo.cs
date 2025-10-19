using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IQuizRepo : IRepository<Quiz>
    {
        //public Task<Quiz?> GetByNameAsync(string Title);
        public Task<Quiz?> GetByIDWithCourse(int ID);
        public Task<List<Quiz>> GetByCourseIdAsync(int courseId);
        public Task<bool> HasResultAsync(int quizId, string studentId);
        public  Task<List<int>> GetCompletedQuizIdsAsync(List<int> quizIds, string studentId);
    }
}
