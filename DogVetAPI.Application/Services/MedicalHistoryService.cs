using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for medical history records
    /// </summary>
    public class MedicalHistoryService(IMedicalHistoryRepository medicalHistoryRepository) : IMedicalHistoryService
    {
        private readonly IMedicalHistoryRepository _medicalHistoryRepository = medicalHistoryRepository ?? throw new ArgumentNullException(nameof(medicalHistoryRepository));

        public async Task<IEnumerable<MedicalHistoryEntity>> GetAllRecordsAsync()
        {
            return await _medicalHistoryRepository.GetAllActiveAsync();
        }

        public async Task<MedicalHistoryEntity?> GetRecordByIdAsync(int id)
        {
            return await _medicalHistoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MedicalHistoryEntity>> GetRecordsByPetIdAsync(int petId)
        {
            return await _medicalHistoryRepository.GetByPetIdAsync(petId);
        }

        public async Task<MedicalHistoryEntity> CreateRecordAsync(MedicalHistoryEntity record)
        {
            record.Status = record.FollowUpDate.HasValue
                ? MedicalHistoryStatusStrings.FollowUp
                : MedicalHistoryStatusStrings.Completed;
            
            // If this is a follow-up record, mark the original record as Completed
            if (record.FollowUpOf.HasValue)
            {
                var previousRecord = await _medicalHistoryRepository.GetByIdAsync(record.FollowUpOf.Value);
                if (previousRecord != null)
                {
                    previousRecord.Status = MedicalHistoryStatusStrings.Completed;
                    previousRecord.UpdatedAt = DateTime.UtcNow;
                    _medicalHistoryRepository.Update(previousRecord);
                }
            }
            
            record.CreatedAt = DateTime.UtcNow;
            record.UpdatedAt = DateTime.UtcNow;
            
            var createdRecord = await _medicalHistoryRepository.AddAsync(record);
            await _medicalHistoryRepository.SaveChangesAsync();
            
            return createdRecord;
        }

        public async Task<MedicalHistoryEntity> UpdateRecordAsync(MedicalHistoryEntity record)
        {
            record.UpdatedAt = DateTime.UtcNow;
            
            var updatedRecord = _medicalHistoryRepository.Update(record);
            await _medicalHistoryRepository.SaveChangesAsync();
            
            return updatedRecord;
        }

        public async Task<bool> DeleteRecordAsync(int id)
        {
            var deleted = await _medicalHistoryRepository.DeleteAsync(id);
            if (deleted)
                await _medicalHistoryRepository.SaveChangesAsync();
            
            return deleted;
        }

        public async Task<bool> SoftDeleteRecordAsync(int id)
        {
            var record = await _medicalHistoryRepository.GetHistoryWithDetailsAsync(id);
            if (record == null)
                return false;

            record.IsActive = false;
            record.UpdatedAt = DateTime.UtcNow;

            
            _medicalHistoryRepository.Update(record);
            await _medicalHistoryRepository.SaveChangesAsync();

            return true;
        }
    }
}

