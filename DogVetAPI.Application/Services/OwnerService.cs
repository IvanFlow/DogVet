using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for owners
    /// </summary>
    public class OwnerService(IOwnerRepository ownerRepository) : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));

        public async Task<IEnumerable<OwnerEntity>> GetAllOwnersAsync()
        {
            return await _ownerRepository.GetAllActiveAsync();
        }

        public async Task<OwnerEntity?> GetOwnerByIdAsync(int id)
        {
            return await _ownerRepository.GetByIdAsync(id);
        }

        public async Task<OwnerEntity> CreateOwnerAsync(OwnerEntity owner)
        {
            owner.CreatedAt = DateTime.UtcNow;
            owner.UpdatedAt = DateTime.UtcNow;
            
            var createdOwner = await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveChangesAsync();
            
            return createdOwner;
        }

        public async Task<OwnerEntity> UpdateOwnerAsync(OwnerEntity owner)
        {
            owner.UpdatedAt = DateTime.UtcNow;
            
            var updatedOwner = _ownerRepository.Update(owner);
            await _ownerRepository.SaveChangesAsync();
            
            return updatedOwner;
        }

        public async Task<bool> DeleteOwnerAsync(int id)
        {
            var deleted = await _ownerRepository.DeleteAsync(id);
            if (deleted)
                await _ownerRepository.SaveChangesAsync();
            
            return deleted;
        }

        public async Task<OwnerEntity?> GetOwnerWithPetsAsync(int id)
        {
            return await _ownerRepository.GetOwnerWithPetsAsync(id);
        }

        public async Task<bool> SoftDeleteOwnerAsync(int id)
        {
            var owner = await _ownerRepository.GetOwnerWithPetsWithMedicalHistoriesAsync(id);
            if (owner == null)
                return false;

            owner.IsActive = false;
            owner.UpdatedAt = DateTime.UtcNow;

            foreach (var pet in owner.Pets)
            {
                pet.IsActive = false;
                pet.UpdatedAt = DateTime.UtcNow;
            }

            foreach(var medicalHistory in owner.Pets.SelectMany(p => p.MedicalHistories))
            {
                medicalHistory.IsActive = false;
                medicalHistory.UpdatedAt = DateTime.UtcNow;
            }

            _ownerRepository.Update(owner);
            await _ownerRepository.SaveChangesAsync();

            return true;
        }
    }
}

