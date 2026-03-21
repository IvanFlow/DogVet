using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for owners
    /// </summary>
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<Owner?> GetOwnerByIdAsync(int id);
        Task<Owner> CreateOwnerAsync(Owner owner);
        Task<Owner> UpdateOwnerAsync(Owner owner);
        Task<bool> DeleteOwnerAsync(int id);
        Task<bool> SoftDeleteOwnerAsync(int id);
        Task<Owner?> GetOwnerWithPetsAsync(int id);
    }
}
