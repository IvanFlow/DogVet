using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for veterinarians
    /// </summary>
    public class VeterinarianService : IVeterinarianService
    {
        private readonly IVeterinarianRepository _veterinarianRepository;

        public VeterinarianService(IVeterinarianRepository veterinarianRepository)
        {
            _veterinarianRepository = veterinarianRepository;
        }

        public async Task<IEnumerable<Veterinarian>> GetAllVeterinariansAsync()
        {
            return await _veterinarianRepository.GetAllAsync();
        }

        public async Task<Veterinarian?> GetVeterinarianByIdAsync(int id)
        {
            return await _veterinarianRepository.GetByIdAsync(id);
        }

        public async Task<Veterinarian> CreateVeterinarianAsync(Veterinarian veterinarian)
        {
            veterinarian.CreatedAt = DateTime.UtcNow;
            veterinarian.UpdatedAt = DateTime.UtcNow;
            
            var createdVeterinarian = await _veterinarianRepository.AddAsync(veterinarian);
            await _veterinarianRepository.SaveChangesAsync();
            
            return createdVeterinarian;
        }

        public async Task<Veterinarian> UpdateVeterinarianAsync(Veterinarian veterinarian)
        {
            veterinarian.UpdatedAt = DateTime.UtcNow;
            
            var updatedVeterinarian = await _veterinarianRepository.UpdateAsync(veterinarian);
            await _veterinarianRepository.SaveChangesAsync();
            
            return updatedVeterinarian;
        }

        public async Task<bool> DeleteVeterinarianAsync(int id)
        {
            var deleted = await _veterinarianRepository.DeleteAsync(id);
            if (deleted)
                await _veterinarianRepository.SaveChangesAsync();
            
            return deleted;
        }

        public async Task<IEnumerable<Veterinarian>> GetActiveVeterinariansAsync()
        {
            return await _veterinarianRepository.GetActiveVeterinariansAsync();
        }
    }
}
