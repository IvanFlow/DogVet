using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for owners
    /// </summary>
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner?> GetOwnerByIdAsync(int id)
        {
            return await _ownerRepository.GetByIdAsync(id);
        }

        public async Task<Owner> CreateOwnerAsync(Owner owner)
        {
            owner.CreatedAt = DateTime.UtcNow;
            owner.UpdatedAt = DateTime.UtcNow;
            
            var createdOwner = await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveChangesAsync();
            
            return createdOwner;
        }

        public async Task<Owner> UpdateOwnerAsync(Owner owner)
        {
            owner.UpdatedAt = DateTime.UtcNow;
            
            var updatedOwner = await _ownerRepository.UpdateAsync(owner);
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

        public async Task<Owner?> GetOwnerWithPetsAsync(int id)
        {
            return await _ownerRepository.GetOwnerWithPetsAsync(id);
        }
    }
}
