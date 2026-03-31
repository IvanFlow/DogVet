using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for owners
    /// </summary>
    public interface IOwnerRepository : IRepository<OwnerEntity>
    {
        Task<OwnerEntity?> GetByEmailAsync(string email);
        Task<IEnumerable<OwnerEntity>> GetOwnersWithPetsAsync();
        Task<OwnerEntity?> GetOwnerWithPetsAsync(int id);
        Task<OwnerEntity?> GetOwnerWithPetsWithMedicalHistoriesAsync(int id);
        Task<IEnumerable<OwnerEntity>> GetAllActiveAsync();
    }
}

