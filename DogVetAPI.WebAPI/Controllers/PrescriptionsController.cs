using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController(IPrescriptionService prescriptionService, ILogger<PrescriptionsController> logger) : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService = prescriptionService ?? throw new ArgumentNullException(nameof(prescriptionService));
        private readonly ILogger<PrescriptionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets all prescriptions for a medical history record
        /// </summary>
        [HttpGet("GetByMedicalHistoryId")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByMedicalHistoryId([FromQuery] int medicalHistoryId)
        {
            try
            {
                var prescriptions = await _prescriptionService.GetByMedicalHistoryIdAsync(medicalHistoryId);
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving prescriptions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates or updates prescriptions for a medical history record
        /// </summary>
        [HttpPost("CreatePrescriptionsByMedicalHistoryId")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> CreatePrescriptionsByMedicalHistoryId([FromBody] CreatePrescriptionsRequest request)
        {
            try
            {
                if (request == null || request.MedicalHistoryId <= 0)
                    return BadRequest("Invalid medical history ID");

                if (request.Prescriptions == null || !request.Prescriptions.Any())
                    return BadRequest("At least one prescription is required");

                

                var result = await _prescriptionService.CreateOrUpdatePrescriptionsAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prescriptions");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all available dose frequency options
        /// </summary>
        [HttpGet("GetDoseFrequencyOptions")]
        public ActionResult<IEnumerable<object>> GetDoseFrequencyOptions()
        {
            try
            {
                var options = Enum.GetValues(typeof(DoseFrequency))
                    .Cast<DoseFrequency>()
                    .Select(d => new { value = d.ToString(), id = (int)d })
                    .OrderBy(x => x.id)
                    .ToList();

                return Ok(options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dose frequency options");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all available prescription status options
        /// </summary>
        [HttpGet("GetPrescriptionStatusOptions")]
        public ActionResult<IEnumerable<object>> GetPrescriptionStatusOptions()
        {
            try
            {
                var options = Enum.GetValues(typeof(PrescriptionStatus))
                    .Cast<PrescriptionStatus>()
                    .Select(s => new { value = s.ToString(), id = (int)s })
                    .OrderBy(x => x.id)
                    .ToList();

                return Ok(options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving prescription status options");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

