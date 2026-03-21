using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for medical history records
    /// </summary>
    public interface IMedicalHistoryService
    {
        Task<IEnumerable<MedicalHistory>> GetAllRecordsAsync();
        Task<MedicalHistory?> GetRecordByIdAsync(int id);
        Task<MedicalHistory> CreateRecordAsync(MedicalHistory record);
        Task<MedicalHistory> UpdateRecordAsync(MedicalHistory record);
        Task<bool> DeleteRecordAsync(int id);
        Task<bool> SoftDeleteRecordAsync(int id);
    }
}
