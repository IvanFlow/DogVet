using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for pets
    /// </summary>
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _petRepository.GetAllAsync();
        }

        public async Task<Pet?> GetPetByIdAsync(int id)
        {
            return await _petRepository.GetByIdAsync(id);
        }

        public async Task<Pet> CreatePetAsync(Pet pet)
        {
            pet.CreatedAt = DateTime.UtcNow;
            pet.UpdatedAt = DateTime.UtcNow;
            
            var createdPet = await _petRepository.AddAsync(pet);
            await _petRepository.SaveChangesAsync();
            
            return createdPet;
        }

        public async Task<Pet> UpdatePetAsync(Pet pet)
        {
            pet.UpdatedAt = DateTime.UtcNow;
            
            var updatedPet = _petRepository.Update(pet);
            await _petRepository.SaveChangesAsync();
            
            return updatedPet;
        }

        public async Task<bool> DeletePetAsync(int id)
        {
            var deleted = await _petRepository.DeleteAsync(id);
            if (deleted)
                await _petRepository.SaveChangesAsync();
            
            return deleted;
        }

        public async Task<IEnumerable<Pet>> GetPetsByOwnerAsync(int ownerId)
        {
            return await _petRepository.GetPetsByOwnerAsync(ownerId);
        }

        public async Task<Pet?> GetPetWithHistoryAsync(int id)
        {
            return await _petRepository.GetPetWithHistoryAsync(id);
        }

        public async Task<bool> SoftDeletePetAsync(int id)
        {
            // Get pet with medical histories
            var pet = await _petRepository.GetPetWithHistoryAsync(id);
            if (pet == null)
                return false;

            // Set pet as inactive
            pet.IsActive = false;
            pet.UpdatedAt = DateTime.UtcNow;

            // Set all medical histories as inactive
            foreach (var medicalHistory in pet.MedicalHistories)
            {
                medicalHistory.IsActive = false;
                medicalHistory.UpdatedAt = DateTime.UtcNow;
            }

            _petRepository.Update(pet);
            await _petRepository.SaveChangesAsync();

            return true;
        }
    }
}
