using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Application.Mappers;
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

        public async Task<IEnumerable<MedicalHistoryDto>> GetAllRecordsAsync()
        {
            var records = await _medicalHistoryRepository.GetAllActiveAsync();
            return records.ToDto();
        }

        public async Task<MedicalHistoryDto?> GetRecordByIdAsync(int id)
        {
            var record = await _medicalHistoryRepository.GetByIdAsync(id);
            return record?.ToDto();
        }

        public async Task<IEnumerable<MedicalHistoryDto>> GetRecordsByPetIdAsync(int petId)
        {
            var records = await _medicalHistoryRepository.GetByPetIdAsync(petId);
            return records.ToDto();
        }

        public async Task<MedicalHistoryDto> CreateRecordAsync(MedicalHistoryEntity record)
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
            
            return createdRecord.ToDto();
        }

        public async Task<MedicalHistoryDto> UpdateRecordAsync(UpdateMedicalHistoryDto updateRecordDto)
        {

            var existingRecordDto = await _medicalHistoryRepository.GetByIdAsync(updateRecordDto.Id);
            
            if (existingRecordDto == null)
                    return null;

            existingRecordDto.Diagnosis = updateRecordDto.Diagnosis;
            existingRecordDto.Notes = updateRecordDto.Notes;
            existingRecordDto.VisitDate = updateRecordDto.VisitDate;
            existingRecordDto.FollowUpDate = updateRecordDto.FollowUpDate;
            existingRecordDto.Status = updateRecordDto.Status;
            existingRecordDto.PetId = updateRecordDto.PetId;
            existingRecordDto.VeterinarianId = updateRecordDto.VeterinarianId;
            existingRecordDto.FollowUpOf = updateRecordDto.FollowUpOf;
            existingRecordDto.UpdatedAt = DateTime.UtcNow;
            
            var updatedRecord = _medicalHistoryRepository.Update(existingRecordDto);
            await _medicalHistoryRepository.SaveChangesAsync();
            
            return updatedRecord.ToDto();
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

