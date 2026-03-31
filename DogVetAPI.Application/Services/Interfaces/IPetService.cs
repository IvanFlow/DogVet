using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for pets
    /// </summary>
    public interface IPetService
    {
        Task<IEnumerable<PetEntity>> GetAllPetsAsync();
        Task<PetEntity?> GetPetByIdAsync(int id);
        Task<PetEntity?> GetPetWithHistoryAsync(int id);
        Task<PetEntity> CreatePetAsync(PetEntity pet);
        Task<PetEntity> UpdatePetAsync(PetEntity pet);
        Task<bool> DeletePetAsync(int id);
        Task<bool> SoftDeletePetAsync(int id);
    }
}

