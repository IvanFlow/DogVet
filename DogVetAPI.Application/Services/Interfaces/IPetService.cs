using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for pets
    /// </summary>
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<Pet?> GetPetByIdAsync(int id);
        Task<Pet> CreatePetAsync(Pet pet);
        Task<Pet> UpdatePetAsync(Pet pet);
        Task<bool> DeletePetAsync(int id);
        Task<bool> SoftDeletePetAsync(int id);
    }
}
