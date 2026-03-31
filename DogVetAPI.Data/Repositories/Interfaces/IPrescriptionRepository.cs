using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for prescriptions
    /// </summary>
    public interface IPrescriptionRepository : IRepository<PrescriptionEntity>
    {
        Task<IEnumerable<PrescriptionEntity>> GetByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<bool> DeleteByMedicalHistoryIdAsync(int medicalHistoryId);
    }
}

