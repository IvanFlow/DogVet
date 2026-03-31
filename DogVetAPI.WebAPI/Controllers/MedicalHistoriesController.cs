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
                var recordDtos = await _medicalHistoryService.GetAllRecordsAsync();
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

                return Ok(record);
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
                var recordDtos = await _medicalHistoryService.GetRecordsByPetIdAsync(petId);
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

                var createdRecordDto = await _medicalHistoryService.CreateRecordAsync(record);
                return CreatedAtAction(nameof(GetRecordById), new { id = createdRecordDto.Id }, createdRecordDto);
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
                var updatedRecord = await _medicalHistoryService.UpdateRecordAsync(updateRecordDto);
                if (updatedRecord == null) 
                    return NotFound($"Medical record with ID {updateRecordDto.Id} not found");
               
                return Ok(updatedRecord);
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
    }
}

