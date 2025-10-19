using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IProjectRepo : IRepository<Project>
    {
        public Task<Project?> GetByIdWithCourseAsync(int ID);
        public Task<List<Project>> GetByCourseIdAsync(int courseId);
    }
}
