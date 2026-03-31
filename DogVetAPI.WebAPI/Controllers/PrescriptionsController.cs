using DogVetAPI.Application;
using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using DogVetAPI.Data.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController(DogVetContext context, ILogger<PrescriptionsController> logger) : ControllerBase
    {
        private readonly DogVetContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly ILogger<PrescriptionsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets all prescriptions for a medical history record
        /// </summary>
        [HttpGet("GetByMedicalHistoryId")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByMedicalHistoryId([FromQuery] int medicalHistoryId)
        {
            try
            {
                var prescriptions = await _context.Prescriptions
                    .Where(p => p.MedicalHistoryId == medicalHistoryId)
                    .ToListAsync();

                var dtos = prescriptions.Select(p => MapToDto(p)).ToList();
                return Ok(dtos);
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

                // Verify that the medical history record exists
                var medicalHistory = await _context.MedicalHistories.FindAsync(request.MedicalHistoryId);
                if (medicalHistory == null)
                    return NotFound("Medical history record not found");

                // Delete existing prescriptions for this medical history
                var existingPrescriptions = await _context.Prescriptions
                    .Where(p => p.MedicalHistoryId == request.MedicalHistoryId)
                    .ToListAsync();
                
                _context.Prescriptions.RemoveRange(existingPrescriptions);
                await _context.SaveChangesAsync();

                // Create new prescriptions
                var newPrescriptions = new List<Prescription>();
                if (request.Prescriptions != null && request.Prescriptions.Any())
                {
                    foreach (var prescriptionRequest in request.Prescriptions)
                    {
                        var prescription = new Prescription
                        {
                            MedName = prescriptionRequest.MedName,
                            Dose = Enum.Parse<DoseFrequency>(prescriptionRequest.Dose),
                            DurationInDays = prescriptionRequest.DurationInDays,
                            Status = Enum.Parse<PrescriptionStatus>(prescriptionRequest.Status),
                            MedicalHistoryId = request.MedicalHistoryId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        newPrescriptions.Add(prescription);
                    }

                    _context.Prescriptions.AddRange(newPrescriptions);
                    await _context.SaveChangesAsync();
                }

                var dtos = newPrescriptions.Select(p => MapToDto(p)).ToList();
                return Ok(dtos);
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

        private PrescriptionDto MapToDto(Prescription prescription)
        {
            return new PrescriptionDto
            {
                Id = prescription.Id,
                MedName = prescription.MedName,
                Dose = prescription.Dose.ToString(),
                DurationInDays = prescription.DurationInDays,
                Status = prescription.Status.ToString(),
                MedicalHistoryId = prescription.MedicalHistoryId
            };
        }
    }

    /// <summary>
    /// Request model for creating prescriptions
    /// </summary>
    public class CreatePrescriptionsRequest
    {
        public int MedicalHistoryId { get; set; }
        public List<CreatePrescriptionItemRequest>? Prescriptions { get; set; }
    }

    /// <summary>
    /// Individual prescription item for creation
    /// </summary>
    public class CreatePrescriptionItemRequest
    {
        public string MedName { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public int DurationInDays { get; set; }
        public string Status { get; set; } = "Prescribed";
    }
}
