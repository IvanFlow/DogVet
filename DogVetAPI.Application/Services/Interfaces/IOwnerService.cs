using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for owners
    /// </summary>
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerEntity>> GetAllOwnersAsync();
        Task<OwnerEntity?> GetOwnerByIdAsync(int id);
        Task<OwnerEntity> CreateOwnerAsync(OwnerEntity owner);
        Task<OwnerEntity> UpdateOwnerAsync(OwnerEntity owner);
        Task<bool> DeleteOwnerAsync(int id);
        Task<bool> SoftDeleteOwnerAsync(int id);
        Task<OwnerEntity?> GetOwnerWithPetsAsync(int id);
    }
}

