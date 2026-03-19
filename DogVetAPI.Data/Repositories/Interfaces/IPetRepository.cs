using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for pets
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        Task<IEnumerable<Pet>> GetPetsByOwnerAsync(int ownerId);
        Task<Pet?> GetPetWithHistoryAsync(int id);
        Task<IEnumerable<Pet>> GetActivePetsAsync();
    }
}
