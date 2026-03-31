using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for pets
    /// </summary>
    public interface IPetRepository : IRepository<PetEntity>
    {
        Task<PetEntity?> GetPetWithHistoryAsync(int id);
        Task<IEnumerable<PetEntity>> GetAllActiveAsync();
    }
}

