using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CompletedMaterialRepo : Repository<CompletedMaterial>, ICompletedMaterialRepo
    {
        ApplicationDBContext _context;

        public CompletedMaterialRepo(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfCompletedAsync(int EnrollmentID, int MaterialID)
        {
            CompletedMaterial? cm = await _context.CompletedMaterials.Where(cmp => cmp.EnrollmentID == EnrollmentID)
                                .Where(cmp => cmp.MaterialID == MaterialID)
                                .FirstOrDefaultAsync();

            return cm != null;
        }

        public async Task DeleteByEnrollmentIdAsync(int enrollmentId)
        {
            var completedMaterials = await _context.CompletedMaterials
                .Where(cm => cm.EnrollmentID == enrollmentId)
                .ToListAsync();
            _context.CompletedMaterials.RemoveRange(completedMaterials);
        }

        public async Task<CompletedMaterial?> GetByEnrollmentIDAndLMID(int EnrollmentID, int MaterialID)
        {
            return await _context.CompletedMaterials.Where(cmp => cmp.EnrollmentID == EnrollmentID)
                    .Where(cmp => cmp.MaterialID == MaterialID)
                    .FirstOrDefaultAsync();
        }
    }
}