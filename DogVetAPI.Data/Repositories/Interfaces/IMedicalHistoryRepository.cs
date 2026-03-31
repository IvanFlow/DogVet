using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for medical history records
    /// </summary>
    public interface IMedicalHistoryRepository : IRepository<MedicalHistoryEntity>
    {
        Task<MedicalHistoryEntity?> GetHistoryWithDetailsAsync(int id);
        Task<IEnumerable<MedicalHistoryEntity>> GetAllActiveAsync();
        Task<IEnumerable<MedicalHistoryEntity>> GetByPetIdAsync(int petId);
    }
}

