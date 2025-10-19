using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IQuestionRepo : IRepository<Question>
    {
        //public Question GetByNameAsync(string QuestionName);
        public Task<List<Question>> GetByQuizIDAsync(int QuizID);
    }
}
