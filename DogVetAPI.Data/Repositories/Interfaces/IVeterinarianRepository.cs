using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for veterinarians
    /// </summary>
    public interface IVeterinarianRepository : IRepository<Veterinarian>
    {
        Task<Veterinarian?> GetByEmailAsync(string email);
        Task<IEnumerable<Veterinarian>> GetActiveVeterinariansAsync();
    }
}
