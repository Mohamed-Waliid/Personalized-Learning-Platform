using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CourseTrackRepo : Repository<CourseTrack>, ICourseTrackRepo
    {
        public CourseTrackRepo(ApplicationDBContext context) : base(context) { }


        public async Task RemoveByCourseIdAsync(int CourseID)
        {
            var courseTracks = _context.CourseTracks.Where(ct => ct.CourseID == CourseID);
            _context.CourseTracks.RemoveRange(courseTracks);
            await _context.SaveChangesAsync();
        }

    }
}
