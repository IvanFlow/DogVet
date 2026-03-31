using DogVetAPI.Data.Models;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for prescriptions
    /// </summary>
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<IEnumerable<Prescription>> GetByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<bool> DeleteByMedicalHistoryIdAsync(int medicalHistoryId);
    }
}
