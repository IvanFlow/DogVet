using DogVetAPI.Application;
using DogVetAPI.Application.Application;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for pets
    /// </summary>
    public interface IPetService
    {
        Task<IEnumerable<PetDto>> GetAllPetsAsync();
        Task<PetDto?> GetPetByIdAsync(int id);
        Task<PetDto?> GetPetWithHistoryAsync(int id);
        Task<PetDto> CreatePetAsync(CreatePetDto createPetDto);
        Task<PetDto?> UpdatePetAsync(UpdatePetDto updatePetDto);
        Task<bool> DeletePetAsync(int id);
        Task<bool> SoftDeletePetAsync(int id);
        IEnumerable<EnumOptionDto> GetSpeciesOptions();
    }
}

