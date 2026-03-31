using DogVetAPI.Application;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for medical history records
    /// </summary>
    public interface IMedicalHistoryService
    {
        Task<IEnumerable<MedicalHistoryDto>> GetAllRecordsAsync();
        Task<MedicalHistoryDto?> GetRecordByIdAsync(int id);
        Task<IEnumerable<MedicalHistoryDto>> GetRecordsByPetIdAsync(int petId);
        Task<MedicalHistoryDto> CreateRecordAsync(MedicalHistoryEntity record);
        Task<MedicalHistoryDto> UpdateRecordAsync(UpdateMedicalHistoryDto updateRecordDto);
        Task<bool> DeleteRecordAsync(int id);
        Task<bool> SoftDeleteRecordAsync(int id);
    }
}

