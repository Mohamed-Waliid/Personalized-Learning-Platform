using Graduation_Project.Data;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IEnrollmentRepo : IRepository<Enrollment>
    {
        Task<int> GetEnrollmentCountAsync(int courseId);
        Task<bool> EnrollStudentAsync(string studentId, int courseId);
        Task<bool> IsStudentEnrolledAsync(string studentId, int courseId);
        Task<IEnumerable<Course>> GetEnrolledCoursesAsync(string studentId);
        Task<List<Enrollment>> GetEnrollmentsWithCoursesAndMaterialsAsync(string studentId);

        public Task<List<Enrollment>> GetByCourseIDAsync(int ID);
        public Task<List<Enrollment>> GetByStudentIDAsync(string StudentID);
        public Task<Enrollment?> GetByCourseIDAndUserIDAsync(int CourseID, string UserID);
    }
}