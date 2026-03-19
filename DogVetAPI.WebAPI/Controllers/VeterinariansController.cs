using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeterinariansController : ControllerBase
    {
        private readonly IVeterinarianService _veterinarianService;
        private readonly ILogger<VeterinariansController> _logger;

        public VeterinariansController(IVeterinarianService veterinarianService, ILogger<VeterinariansController> logger)
        {
            _veterinarianService = veterinarianService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all veterinarians
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetAllVeterinarians()
        {
            try
            {
                var veterinarians = await _veterinarianService.GetAllVeterinariansAsync();
                var veterinarianDtos = veterinarians.Select(v => MapToDto(v)).ToList();
                return Ok(veterinarianDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving veterinarians");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a veterinarian by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VeterinarianDto>> GetVeterinarianById(int id)
        {
            try
            {
                var veterinarian = await _veterinarianService.GetVeterinarianByIdAsync(id);
                if (veterinarian == null)
                    return NotFound($"Veterinarian with ID {id} not found");

                return Ok(MapToDto(veterinarian));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving veterinarian");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets active veterinarians
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<VeterinarianDto>>> GetActiveVeterinarians()
        {
            try
            {
                var veterinarians = await _veterinarianService.GetActiveVeterinariansAsync();
                var veterinarianDtos = veterinarians.Select(v => MapToDto(v)).ToList();
                return Ok(veterinarianDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active veterinarians");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new veterinarian
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<VeterinarianDto>> CreateVeterinarian([FromBody] CreateVeterinarianDto createVeterinarianDto)
        {
            try
            {
                var veterinarian = new Veterinarian
                {
                    FirstName = createVeterinarianDto.FirstName,
                    LastName = createVeterinarianDto.LastName,
                    Email = createVeterinarianDto.Email,
                    PhoneNumber = createVeterinarianDto.PhoneNumber
                };

                var createdVeterinarian = await _veterinarianService.CreateVeterinarianAsync(veterinarian);
                return CreatedAtAction(nameof(GetVeterinarianById), new { id = createdVeterinarian.Id }, MapToDto(createdVeterinarian));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating veterinarian");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing veterinarian
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<VeterinarianDto>> UpdateVeterinarian(int id, [FromBody] UpdateVeterinarianDto updateVeterinarianDto)
        {
            try
            {
                var existingVeterinarian = await _veterinarianService.GetVeterinarianByIdAsync(id);
                if (existingVeterinarian == null)
                    return NotFound($"Veterinarian with ID {id} not found");

                existingVeterinarian.FirstName = updateVeterinarianDto.FirstName;
                existingVeterinarian.LastName = updateVeterinarianDto.LastName;
                existingVeterinarian.Email = updateVeterinarianDto.Email;
                existingVeterinarian.PhoneNumber = updateVeterinarianDto.PhoneNumber;
                existingVeterinarian.IsActive = updateVeterinarianDto.IsActive;

                var updatedVeterinarian = await _veterinarianService.UpdateVeterinarianAsync(existingVeterinarian);
                return Ok(MapToDto(updatedVeterinarian));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating veterinarian");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a veterinarian
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeterinarian(int id)
        {
            try
            {
                var deleted = await _veterinarianService.DeleteVeterinarianAsync(id);
                if (!deleted)
                    return NotFound($"Veterinarian with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting veterinarian");
                return StatusCode(500, "Internal server error");
            }
        }

        private VeterinarianDto MapToDto(Veterinarian veterinarian)
        {
            return new VeterinarianDto
            {
                Id = veterinarian.Id,
                FirstName = veterinarian.FirstName,
                LastName = veterinarian.LastName,
                Email = veterinarian.Email,
                PhoneNumber = veterinarian.PhoneNumber,
                IsActive = veterinarian.IsActive
            };
        }
    }
}
