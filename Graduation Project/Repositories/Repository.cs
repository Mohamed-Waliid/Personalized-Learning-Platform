using Graduation_Project.Data;
using Graduation_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly ApplicationDBContext _context;
        protected readonly DbSet<T> _dbset;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _dbset.FindAsync(Id);
        }

        public async Task<T> GetByNameAsync(string Name, string PropertyName = "Name")
        {
            var property = typeof(T).GetProperty(PropertyName);

            //if (property == null)
            //    throw new ArgumentException($"Property '{PropertyName}' not found in {typeof(T).Name}");

            return await _dbset.FirstOrDefaultAsync(e =>
                EF.Property<string>(e, PropertyName).ToLower() == Name.ToLower());
        }

        public async Task CreateAsync(T entity)
        {
            await _dbset.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbset.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbset.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
