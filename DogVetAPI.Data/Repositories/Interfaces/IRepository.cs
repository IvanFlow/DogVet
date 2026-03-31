using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync();
    }
}

