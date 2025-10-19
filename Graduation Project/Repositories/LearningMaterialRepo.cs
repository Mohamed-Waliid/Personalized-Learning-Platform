using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class LearningMaterialRepo : Repository<LearningMaterial>, ILearningMaterialRepo
    {
        ApplicationDBContext _context;

        public LearningMaterialRepo(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<LearningMaterial>> GetByCourseIdAsync(int courseId)
        {
            return _context.LearningMaterials.Where(l => l.ID == courseId).ToList();
        }

        public async Task<List<LearningMaterial>> GetByCourseIdWithCompletionsAsync(int courseId)
        {
            return await _context.LearningMaterials
            .Where(m => m.CourseMaterials.Any(cm => cm.CourseID == courseId))
            .Include(m => m.CompletedMaterials)
                .ThenInclude(cm => cm.Enrollment)
                    .ThenInclude(e => e.Student) // Matches Enrollment.Student
            .ToListAsync();
        }
    }
}
