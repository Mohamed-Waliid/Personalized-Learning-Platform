using Graduation_Project.Data;
using Graduation_Project.Models;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ICompletedMaterialRepo : IRepository<CompletedMaterial>
    {
        Task DeleteByEnrollmentIdAsync(int enrollmentId);
        public Task<bool> CheckIfCompletedAsync(int EnrollmentID, int MaterialID);
        public Task<CompletedMaterial?> GetByEnrollmentIDAndLMID(int EnrollmentID, int MaterialID);
    }
}