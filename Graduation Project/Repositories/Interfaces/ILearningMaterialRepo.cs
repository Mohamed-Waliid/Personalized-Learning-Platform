using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ILearningMaterialRepo : IRepository<LearningMaterial>
    {
        public Task<List<LearningMaterial>> GetByCourseIdAsync(int courseId);
        public Task<List<LearningMaterial>> GetByCourseIdWithCompletionsAsync(int courseId);
    }
}
