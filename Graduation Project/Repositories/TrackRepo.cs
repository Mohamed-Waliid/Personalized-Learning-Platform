using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class TrackRepo : Repository<Track>, ITrackRepo
    {
        public TrackRepo(ApplicationDBContext Context) : base(Context) { }

        public async Task<IEnumerable<Track>> GetAllWithCoursesAsync()
        {
            return await _context.Tracks
                .Include(t => t.CourseTracks)       // Include related CourseTracks
                .ToListAsync();
        }

        public async Task<Track> GetByIdWithCoursesAsync(int id)
        {
            return await _context.Tracks
                        .Include(t => t.CourseTracks)
                        .FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<List<Course>> GetCoursesAsync(int ID)
        {
            return await _context.CourseTracks.Where(ct => ct.TrackID == ID).Select(ct => ct.Course).ToListAsync();
        }
    }
}
