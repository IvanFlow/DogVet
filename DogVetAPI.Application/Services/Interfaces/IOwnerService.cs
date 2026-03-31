using DogVetAPI.Application;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for owners
    /// </summary>
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
        Task<OwnerDto?> GetOwnerByIdAsync(int id);
        Task<OwnerDto> CreateOwnerAsync(OwnerEntity owner);
        Task<OwnerDto?> UpdateOwnerAsync(UpdateOwnerDto updateOwnerDto);
        Task<bool> DeleteOwnerAsync(int id);
        Task<bool> SoftDeleteOwnerAsync(int id);
        Task<OwnerDto?> GetOwnerWithPetsAsync(int id);
    }
}

