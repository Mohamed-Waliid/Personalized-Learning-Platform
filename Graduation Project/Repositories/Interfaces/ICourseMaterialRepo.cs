using Graduation_Project.Data;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ICourseMaterialRepo : IRepository<CourseMaterial>
    {
        public Task<Course> GetCourseByMatID(int ID);
        public Task<List<LearningMaterial>?> GetByCourseIDAsync(int ID);
    }
}