using Graduation_Project.Models;
using System.Threading.Tasks;

namespace Graduation_Project.Repositories.Interfaces
{
    public interface ICourseTrackRepo : IRepository<CourseTrack>
    {
        Task RemoveByCourseIdAsync(int CourseID);
    }
}
