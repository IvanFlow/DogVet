using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for medical history records
    /// </summary>
    public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
    {
        Task<MedicalHistory?> GetHistoryWithDetailsAsync(int id);
        Task<IEnumerable<MedicalHistory>> GetAllActiveAsync();
        Task<IEnumerable<MedicalHistory>> GetByPetIdAsync(int petId);
    }
}
