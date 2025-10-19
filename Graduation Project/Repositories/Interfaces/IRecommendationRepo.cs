using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IRecommendationRepo : IRepository<Recommendation> {
        public Task<Recommendation?> GetLastRecommendationAsync(string ID);
    }
}
