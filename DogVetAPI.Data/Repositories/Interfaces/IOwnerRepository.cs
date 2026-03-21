using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for owners
    /// </summary>
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<Owner?> GetByEmailAsync(string email);
        Task<IEnumerable<Owner>> GetOwnersWithPetsAsync();
        Task<Owner?> GetOwnerWithPetsAsync(int id);
        Task<Owner?> GetOwnerWithPetsWithMedicalHistoriesAsync(int id);
    }
}
