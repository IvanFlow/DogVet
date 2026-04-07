using DogVetAPI.Application.Application;
using DogVetAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleNotesController(ISaleNoteService saleNoteService, ILogger<SaleNotesController> logger) : ControllerBase
    {
        private readonly ISaleNoteService _saleNoteService = saleNoteService ?? throw new ArgumentNullException(nameof(saleNoteService));
        private readonly ILogger<SaleNotesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Creates a new sale note with concepts
        /// </summary>
        [HttpPost("CreateSaleNote")]
        public async Task<ActionResult<SaleNoteDto>> CreateSaleNote([FromBody] CreateSaleNoteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var saleNote = await _saleNoteService.CreateAsync(request);
                return CreatedAtAction(nameof(GetSaleNoteById), new { id = saleNote.Id }, saleNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sale note");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a sale note by ID
        /// </summary>
        [HttpGet("GetSaleNoteById")]
        public async Task<ActionResult<SaleNoteDto>> GetSaleNoteById([FromQuery] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid sale note ID");

                var saleNote = await _saleNoteService.GetByIdAsync(id);
                if (saleNote == null)
                    return NotFound($"Nota de venta con ID {id} no encontrada");

                return Ok(saleNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale note");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all sale notes for a medical history record
        /// </summary>
        [HttpGet("GetSaleNotesByMedicalHistoryId")]
        public async Task<ActionResult<IEnumerable<SaleNoteDto>>> GetSaleNotesByMedicalHistoryId([FromQuery] int medicalHistoryId)
        {
            try
            {
                if (medicalHistoryId <= 0)
                    return BadRequest("Invalid medical history ID");

                var saleNotes = await _saleNoteService.GetByMedicalHistoryIdAsync(medicalHistoryId);
                return Ok(saleNotes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale notes");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a sale note by ID
        /// </summary>
        [HttpDelete("DeleteSaleNote")]
        public async Task<IActionResult> DeleteSaleNote([FromQuery] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid sale note ID");

                var success = await _saleNoteService.DeleteAsync(id);
                if (!success)
                    return NotFound($"Nota de venta con ID {id} no encontrada");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sale note");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
