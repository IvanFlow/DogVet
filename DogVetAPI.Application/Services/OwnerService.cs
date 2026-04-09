using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Application.Mappers;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for owners
    /// </summary>
    public class OwnerService(IOwnerRepository ownerRepository) : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));

        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllActiveAsync();
            return owners.ToDto();
        }

        public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            return owner?.ToDto();
        }

        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto)
        {
            var owner = new OwnerEntity
                {
                    FirstName = createOwnerDto.FirstName,
                    LastName = createOwnerDto.LastName,
                    Email = createOwnerDto.Email,
                    PhoneNumber = createOwnerDto.PhoneNumber,
                    Address = createOwnerDto.Address,
                    City = createOwnerDto.City,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            
            var createdOwner = await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveChangesAsync();
            
            return createdOwner.ToDto()!;
        }

        public async Task<OwnerDto?> UpdateOwnerAsync(UpdateOwnerDto updateOwnerDto)
        {
            var existingOwner = await _ownerRepository.GetByIdAsync(updateOwnerDto.Id);

            if (existingOwner == null)
                return null;    

            existingOwner.FirstName = updateOwnerDto.FirstName;
            existingOwner.LastName = updateOwnerDto.LastName;
            existingOwner.Email = updateOwnerDto.Email;
            existingOwner.PhoneNumber = updateOwnerDto.PhoneNumber;
            existingOwner.Address = updateOwnerDto.Address;
            existingOwner.City = updateOwnerDto.City;
            existingOwner.UpdatedAt = DateTime.UtcNow;
            
            var updatedOwner = _ownerRepository.Update(existingOwner);
            await _ownerRepository.SaveChangesAsync();
            
            return updatedOwner.ToDto();
        }

        public async Task<bool> DeleteOwnerAsync(int id)
        {
            var deleted = await _ownerRepository.DeleteAsync(id);
            if (deleted)
                await _ownerRepository.SaveChangesAsync();
            
            return deleted;
        }

        public async Task<OwnerDto?> GetOwnerWithPetsAsync(int id)
        {
            var ownerEntity = await _ownerRepository.GetOwnerWithPetsAsync(id);
            return ownerEntity?.ToDto(withPets: true);
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

