using Graduation_Project.Data;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Graduation_Project.Repositories.Interfaces;

namespace Graduation_Project.Repositories
{
    public class ProjectRepo : Repository<Project>, IProjectRepo
    {
        ApplicationDBContext context;

        public ProjectRepo(ApplicationDBContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Project>> GetByCourseIdAsync(int courseId)
        {
            return await context.Projects.Where(p => p.CourseID == courseId).ToListAsync();
        }

        public async Task<Project?> GetByIdWithCourseAsync(int ID)
        {
            return await context.Projects.Include(p => p.Course).Where(p => p.ID == ID).FirstOrDefaultAsync();
        }
    }
}
