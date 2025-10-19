using Graduation_Project.Data;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Repositories.Interfaces;

namespace Graduation_Project.Repositories
{
    public class CourseRepo : Repository<Course>, ICourseRepo
    {
        public CourseRepo(ApplicationDBContext context) : base(context) {}

        public async Task<Course?> GetByIDWithQuizzesAsync(int CourseID)
        {
            return await _context.Courses.Include(c => c.Quizzes).FirstOrDefaultAsync(c => c.ID == CourseID);
        }

        public async Task<Course> GetByIdWithTracksAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.CourseTracks)
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<ICollection<Course>> GetWithEnrollmentsAsync(int cnt)
        {
            if (cnt == 0)
            {
                return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.Enrollments.Count)
                .ToListAsync();
            }

            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(cnt)
                .ToListAsync();
        }

        public async Task<Course> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .Include(c => c.CourseTracks)
                .ThenInclude(ct => ct.Track)
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<ICollection<Course>> GetByTrackIdAsync(int trackId)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.CourseTracks)
                .ThenInclude(ct => ct.Track)
                .Where(c => c.CourseTracks.Any(ct => ct.TrackID == trackId))
                .ToListAsync();
        }

        public async Task<Course?> GetWithEnrollments(int ID)
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .Where(c => c.ID == ID)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Course>?> GetByInstructorIDAsync(string ID, bool WithEnrollments)
        {
            if (WithEnrollments)
            {
                return await _context.Courses.Include(c => c.Enrollments).Where(c => c.InstructorID == ID).ToListAsync();
            }

            return await _context.Courses.Where(c => c.InstructorID == ID).ToListAsync();
        }
    }
}
