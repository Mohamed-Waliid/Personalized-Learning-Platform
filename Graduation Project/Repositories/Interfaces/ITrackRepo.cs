using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ITrackRepo : IRepository<Track>
    {
        public Task<List<Course>> GetCoursesAsync(int ID);
        public Task<Track> GetByIdWithCoursesAsync(int ID);
        public Task<IEnumerable<Track>> GetAllWithCoursesAsync();
    }
}
