using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for pets
    /// </summary>
    public class PetService(IPetRepository petRepository) : IPetService
    {
        private readonly IPetRepository _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));

        public async Task<IEnumerable<PetEntity>> GetAllPetsAsync()
        {
            return await _petRepository.GetAllActiveAsync();
        }

        public async Task<PetEntity?> GetPetByIdAsync(int id)
        {
            return await _petRepository.GetByIdAsync(id);
        }

        public async Task<PetEntity?> GetPetWithHistoryAsync(int id)
        {
            return await _petRepository.GetPetWithHistoryAsync(id);
        }

        public async Task<PetEntity> CreatePetAsync(PetEntity pet)
        {
            pet.CreatedAt = DateTime.UtcNow;
            pet.UpdatedAt = DateTime.UtcNow;
            
            var createdPet = await _petRepository.AddAsync(pet);
            await _petRepository.SaveChangesAsync();
            
            return createdPet;
        }

        public async Task<PetEntity> UpdatePetAsync(PetEntity pet)
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

