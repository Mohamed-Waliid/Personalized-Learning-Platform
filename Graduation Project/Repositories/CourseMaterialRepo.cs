using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CourseMaterialRepo : Repository<CourseMaterial>, ICourseMaterialRepo
    {
        ApplicationDBContext _context;

        public CourseMaterialRepo(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<LearningMaterial>?> GetByCourseIDAsync(int ID)
        {
            return await _context.CourseMaterials.Where(cm => cm.CourseID == ID).Select(cm => cm.LearningMaterial).ToListAsync();
        }

        public async Task<Course> GetCourseByMatID(int ID)
        {
            CourseMaterial c = await _context.CourseMaterials.Include(cm => cm.Course).FirstOrDefaultAsync(cm => cm.MaterialID == ID);
            return c.Course;
        }
    }
}