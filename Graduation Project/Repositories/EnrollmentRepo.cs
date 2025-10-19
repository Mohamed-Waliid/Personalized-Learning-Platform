using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class EnrollmentRepo : Repository<Enrollment>, IEnrollmentRepo
    {
        private readonly ApplicationDBContext _context;
        public EnrollmentRepo(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> EnrollStudentAsync(string studentId, int courseId)
        {
            if (await IsStudentEnrolledAsync(studentId, courseId))
            {
                return false;   // Already enrolled
            }

            var enrollment = new Enrollment
            {
                StudentID = studentId,
                CourseID = courseId,
                EnrollmentDate = DateTime.UtcNow
            };

            await CreateAsync(enrollment);
            return true;
        }

        public async Task<bool> IsStudentEnrolledAsync(string studentId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentID == studentId && e.CourseID == courseId);
        }

        public async Task<IEnumerable<Course>> GetEnrolledCoursesAsync(string studentId)
        {
            return await _context.Enrollments
                .Where(e => e.StudentID == studentId)
                .Include(e => e.Course)
                .Select(e => e.Course)
                .ToListAsync();
        }

        public async Task<int> GetEnrollmentCountAsync(int courseId)
        {
            return await _context.Enrollments
                .CountAsync(e => e.CourseID == courseId);
        }

        public async Task<List<Enrollment>> GetEnrollmentsWithCoursesAndMaterialsAsync(string studentId)
        {
            return await _context.Enrollments
                .Where(e => e.Student.Id == studentId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseMaterials)
                .Include(e => e.CompletedMaterials)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetByStudentIDAsync(string StudentID)
        {
            return await _context.Enrollments.Where(e => e.StudentID == StudentID).ToListAsync();
        }

        public async Task<List<Enrollment>> GetByCourseIDAsync(int ID)
        {
            return await _context.Enrollments.Where(e => e.CourseID == ID).ToListAsync();
        }

        public async Task<Enrollment?> GetByCourseIDAndUserIDAsync(int CourseID, string UserID)
        {
            return await _context.Enrollments.Where(e => e.CourseID == CourseID)
                    .Where(e => e.StudentID == UserID).FirstOrDefaultAsync();
        }

        //public bool IsStudentEnrolledInCourse(int ID, string StudentID)
        //{
        //    return _context.Enrollments.Any(e => e.ID == ID && e.StudentID == StudentID);
        //}

        //public Enrollment GetByIDAndStudentID(int ID, string StudentID)
        //{
        //    return _context.Enrollments.FirstOrDefault(e => e.StudentID == StudentID && e.ID == ID);
        //}

        //public async Task<List<Course>?> GetCoursesByStudentID(string StudentID) {
        //    return await _context.Enrollments.Where(e => e.StudentID == StudentID)
        //                .Select(e => e.Course)
        //                .ToListAsync();
        //}
    }
}
