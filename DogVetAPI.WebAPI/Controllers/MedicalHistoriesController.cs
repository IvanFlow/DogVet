using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalHistoriesController(IMedicalHistoryService medicalHistoryService, ILogger<MedicalHistoriesController> logger) : ControllerBase
    {
        private readonly IMedicalHistoryService _medicalHistoryService = medicalHistoryService ?? throw new ArgumentNullException(nameof(medicalHistoryService));
        private readonly ILogger<MedicalHistoriesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets all medical history records
        /// </summary>
        [HttpGet("GetAllRecords")]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDto>>> GetAllRecords()
        {
            try
            {
                var records = await _medicalHistoryService.GetAllRecordsAsync();
                var recordDtos = records.Select(r => MapToDto(r)).ToList();
                return Ok(recordDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medical records");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a medical history record by ID
        /// </summary>
        [HttpGet("GetRecordById")]
        public async Task<ActionResult<MedicalHistoryDto>> GetRecordById([FromQuery]int id)
        {
            try
            {
                var record = await _medicalHistoryService.GetRecordByIdAsync(id);
                if (record == null)
                    return NotFound($"Medical record with ID {id} not found");

                return Ok(MapToDto(record, includeFollowUpOfRecord: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medical record");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets medical history records by Pet ID
        /// </summary>
        [HttpGet("GetRecordsByPetId")]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDto>>> GetRecordsByPetId([FromQuery]int petId)
        {
            try
            {
                var records = await _medicalHistoryService.GetRecordsByPetIdAsync(petId);
                var recordDtos = records.Select(r => MapToDto(r)).ToList();
                return Ok(recordDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving medical records for pet");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new medical history record
        /// </summary>
        [HttpPost("CreateRecord")]
        public async Task<ActionResult<MedicalHistoryDto>> CreateRecord([FromBody] CreateMedicalHistoryDto createRecordDto)
        {
            try
            {
                var record = new MedicalHistoryEntity
                {
                    Diagnosis = createRecordDto.Diagnosis,
                    Notes = createRecordDto.Notes,
                    VisitDate = createRecordDto.VisitDate,
                    FollowUpDate = createRecordDto.FollowUpDate,
                    PetId = createRecordDto.PetId,
                    FollowUpOf = createRecordDto.FollowUpOf
                };

                var createdRecord = await _medicalHistoryService.CreateRecordAsync(record);
                return CreatedAtAction(nameof(GetRecordById), new { id = createdRecord.Id }, MapToDto(createdRecord));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medical record");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing medical history record
        /// </summary>
        [HttpPut("UpdateRecord")]
        public async Task<ActionResult<MedicalHistoryDto>> UpdateRecord([FromBody] UpdateMedicalHistoryDto updateRecordDto)
        {
            try
            {
                var existingRecord = await _medicalHistoryService.GetRecordByIdAsync(updateRecordDto.Id);
                if (existingRecord == null)
                    return NotFound($"Medical record with ID {updateRecordDto.Id} not found");

                existingRecord.Diagnosis = updateRecordDto.Diagnosis;
                existingRecord.Notes = updateRecordDto.Notes;
                existingRecord.VisitDate = updateRecordDto.VisitDate;
                existingRecord.FollowUpDate = updateRecordDto.FollowUpDate;
                existingRecord.Status = updateRecordDto.Status;
                existingRecord.PetId = updateRecordDto.PetId;
                existingRecord.VeterinarianId = updateRecordDto.VeterinarianId;
                existingRecord.FollowUpOf = updateRecordDto.FollowUpOf;

                var updatedRecord = await _medicalHistoryService.UpdateRecordAsync(existingRecord);
                return Ok(MapToDto(updatedRecord));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating medical record");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a medical history record
        /// </summary>
        [HttpDelete("DeleteRecord")]
        public async Task<IActionResult> DeleteRecord([FromQuery]int id)
        {
            try
            {
                var deleted = await _medicalHistoryService.DeleteRecordAsync(id);
                if (!deleted)
                    return NotFound($"Medical record with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting medical record");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Soft deletes a medical history record (sets IsActive to false)
        /// </summary>
        [HttpDelete("SoftDeleteRecord")]
        public async Task<IActionResult> SoftDeleteRecord([FromQuery]int id)
        {
            try
            {
                var deleted = await _medicalHistoryService.SoftDeleteRecordAsync(id);
                if (!deleted)
                    return NotFound($"Medical record with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting medical record");
                return StatusCode(500, "Internal server error");
            }
        }

        private MedicalHistoryDto MapToDto(MedicalHistoryEntity record, bool includeFollowUpOfRecord = false)
        {
            var dto = new MedicalHistoryDto
            {
                Id = record.Id,
                Diagnosis = record.Diagnosis,
                Notes = record.Notes,
                VisitDate = record.VisitDate,
                FollowUpDate = record.FollowUpDate,
                Status = record.Status,
                PetId = record.PetId,
                VeterinarianId = record.VeterinarianId,
                FollowUpOf = record.FollowUpOf
            };

            if (record.Pet != null)
            {
                dto.Pet = new PetDto
                {
                    Id = record.Pet.Id,
                    Name = record.Pet.Name,
                    Breed = record.Pet.Breed,
                    Weight = record.Pet.Weight,
                    Color = record.Pet.Color,
                    Gender = record.Pet.Gender,
                    DateOfBirth = record.Pet.DateOfBirth,
                    Species = record.Pet.Species,
                    IsActive = record.Pet.IsActive,
                    OwnerId = record.Pet.OwnerId,
                    CreatedAt = record.Pet.CreatedAt,
                    UpdatedAt = record.Pet.UpdatedAt
                };
            }

            if (record.Prescriptions != null && record.Prescriptions.Any())
            {
                dto.Prescriptions = record.Prescriptions.Select(p => new PrescriptionDto
                {
                    Id = p.Id,
                    MedName = p.MedName,
                    Dose = p.Dose.ToString(),
                    DurationInDays = p.DurationInDays,
                    Status = p.Status.ToString(),
                    MedicalHistoryId = p.MedicalHistoryId
                }).ToList();
            }

            if (includeFollowUpOfRecord && record.FollowUpOfRecord != null)
            {
                dto.FollowUpOfRecord = new MedicalHistoryDto
                {
                    Id = record.FollowUpOfRecord.Id,
                    Diagnosis = record.FollowUpOfRecord.Diagnosis,
                    Notes = record.FollowUpOfRecord.Notes,
                    VisitDate = record.FollowUpOfRecord.VisitDate,
                    FollowUpDate = record.FollowUpOfRecord.FollowUpDate,
                    Status = record.FollowUpOfRecord.Status,
                    PetId = record.FollowUpOfRecord.PetId,
                    VeterinarianId = record.FollowUpOfRecord.VeterinarianId,
                    FollowUpOf = record.FollowUpOfRecord.FollowUpOf
                };
            }

            return dto;
        }
    }
}

