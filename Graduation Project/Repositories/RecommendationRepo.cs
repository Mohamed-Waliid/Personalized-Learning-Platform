using Graduation_Project.Data;
using Graduation_Project.Models;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class RecommendationRepo : Repository<Recommendation>, IRecommendationRepo
    {
        public RecommendationRepo(ApplicationDBContext context) : base(context) { }

        public async Task<Recommendation?> GetLastRecommendationAsync(string ID)
        {
            return await _context.Recommendations
                .AsNoTracking()
                .Include(r => r.Track)
                .Where(r => r.StudentID == ID)
                .OrderByDescending(r => r.ID)
                .FirstOrDefaultAsync();
        }
    }
}