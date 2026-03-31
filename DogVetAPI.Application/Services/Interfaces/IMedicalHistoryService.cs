using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for medical history records
    /// </summary>
    public interface IMedicalHistoryService
    {
        Task<IEnumerable<MedicalHistoryEntity>> GetAllRecordsAsync();
        Task<MedicalHistoryEntity?> GetRecordByIdAsync(int id);
        Task<IEnumerable<MedicalHistoryEntity>> GetRecordsByPetIdAsync(int petId);
        Task<MedicalHistoryEntity> CreateRecordAsync(MedicalHistoryEntity record);
        Task<MedicalHistoryEntity> UpdateRecordAsync(MedicalHistoryEntity record);
        Task<bool> DeleteRecordAsync(int id);
        Task<bool> SoftDeleteRecordAsync(int id);
    }
}

