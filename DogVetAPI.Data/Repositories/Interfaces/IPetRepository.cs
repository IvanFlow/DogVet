using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for pets
    /// </summary>
    public interface IPetRepository : IRepository<Pet>
    {
        Task<Pet?> GetPetWithHistoryAsync(int id);
        Task<IEnumerable<Pet>> GetAllActiveAsync();
    }
}
