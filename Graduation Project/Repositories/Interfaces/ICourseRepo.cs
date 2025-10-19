using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ICourseRepo : IRepository<Course>
    {
        public Task<Course> GetByIdWithTracksAsync(int id);
        public Task<Course?> GetByIDWithQuizzesAsync(int CourseID);

        public Task<ICollection<Course>> GetWithEnrollmentsAsync(int cnt);
        public Task<ICollection<Course>> GetByTrackIdAsync(int TrackID);
        public Task<Course?> GetWithEnrollments(int ID);
        public Task<List<Course>?> GetByInstructorIDAsync(string ID, bool WithEnrollments);

        public Task<Course> GetByIdWithDetailsAsync(int id);
    }
}
