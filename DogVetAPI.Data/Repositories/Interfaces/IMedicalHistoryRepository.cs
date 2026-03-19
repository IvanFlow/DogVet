using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for medical history records
    /// </summary>
    public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
    {
        Task<IEnumerable<MedicalHistory>> GetHistoryByPetAsync(int petId);
        Task<IEnumerable<MedicalHistory>> GetHistoryByVeterinarianAsync(int veterinarianId);
        Task<IEnumerable<MedicalHistory>> GetHistoryByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<MedicalHistory?> GetHistoryWithDetailsAsync(int id);
    }
}
