using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for veterinarians
    /// </summary>
    public interface IVeterinarianService
    {
        Task<IEnumerable<Veterinarian>> GetAllVeterinariansAsync();
        Task<Veterinarian?> GetVeterinarianByIdAsync(int id);
        Task<Veterinarian> CreateVeterinarianAsync(Veterinarian veterinarian);
        Task<Veterinarian> UpdateVeterinarianAsync(Veterinarian veterinarian);
        Task<bool> DeleteVeterinarianAsync(int id);
        Task<IEnumerable<Veterinarian>> GetActiveVeterinariansAsync();
    }
}
