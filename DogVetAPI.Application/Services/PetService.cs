using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Application.Mappers;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for pets
    /// </summary>
    public class PetService(IPetRepository petRepository) : IPetService
    {
        private readonly IPetRepository _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));

        public async Task<IEnumerable<PetDto>> GetAllPetsAsync()
        {
            var pets = await _petRepository.GetAllActiveAsync();
            return pets.ToDto();
        }

        public async Task<PetDto?> GetPetByIdAsync(int id)
        {
            var pet = await _petRepository.GetByIdAsync(id);
            return pet?.ToDto();
        }

        public async Task<PetDto?> GetPetWithHistoryAsync(int id)
        {
            var pet = await _petRepository.GetPetWithHistoryAsync(id);
            return pet?.ToDto(withHistory: true);
        }

        public async Task<PetDto> CreatePetAsync(CreatePetDto createPetDto)
        {
               var pet = new PetEntity
                {
                    Name = createPetDto.Name,
                    Breed = createPetDto.Breed,
                    Weight = createPetDto.Weight,
                    Color = createPetDto.Color,
                    Gender = createPetDto.Gender,
                    DateOfBirth = createPetDto.DateOfBirth,
                    Species = createPetDto.Species,
                    OwnerId = createPetDto.OwnerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            
            var createdPet = await _petRepository.AddAsync(pet);
            await _petRepository.SaveChangesAsync();
            
            return createdPet.ToDto();
        }

        public async Task<PetDto> UpdatePetAsync(UpdatePetDto updatePetDto)
        {

             var existingPet = await _petRepository.GetByIdAsync(updatePetDto.Id);

            if (existingPet == null)
            return null; 

                existingPet.Name = updatePetDto.Name;
                existingPet.Breed = updatePetDto.Breed;
                existingPet.Weight = updatePetDto.Weight;
                existingPet.Color = updatePetDto.Color;
                existingPet.Gender = updatePetDto.Gender;
                existingPet.DateOfBirth = updatePetDto.DateOfBirth;
                existingPet.Species = updatePetDto.Species;
                existingPet.IsActive = updatePetDto.IsActive;
                existingPet.OwnerId = updatePetDto.OwnerId;
                existingPet.UpdatedAt = DateTime.UtcNow;
            
            var updatedPet = _petRepository.Update(existingPet);
            await _petRepository.SaveChangesAsync();
            
            return updatedPet.ToDto();
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

