

namespace Graduation_Project.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int Id);

        Task<T> GetByNameAsync(string Name, string PropertyName = "Name");

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

    }
}
