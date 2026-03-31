using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController(IOwnerService ownerService, ILogger<OwnersController> logger) : ControllerBase
    {
        private readonly IOwnerService _ownerService = ownerService ?? throw new ArgumentNullException(nameof(ownerService));
        private readonly ILogger<OwnersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets all owners
        /// </summary>
        [HttpGet("GetAllOwners")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
        {
            try
            {
                var ownerDtos = await _ownerService.GetAllOwnersAsync();
                return Ok(ownerDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving owners");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets an owner by ID
        /// </summary>
        [HttpGet("GetOwnerById")]
        public async Task<ActionResult<OwnerDto>> GetOwnerById([FromQuery]int id)
        {
            try
            {
                var owner = await _ownerService.GetOwnerByIdAsync(id);
                if (owner == null)
                    return NotFound($"Propietario con ID {id} no encontrado");

                return Ok(owner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving owner");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets an owner with their pets
        /// </summary>
        [HttpGet("GetOwnerWithPets")]
        public async Task<ActionResult<OwnerDto>> GetOwnerWithPets([FromQuery]int id)
        {
            try
            {
                var owner = await _ownerService.GetOwnerWithPetsAsync(id);
                if (owner == null)
                    return NotFound($"Owner with ID {id} not found");

                return Ok(owner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving owner with pets");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new owner
        /// </summary>
        [HttpPost("CreateOwner")]
        public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] CreateOwnerDto createOwnerDto)
        {
            try
            {
                var owner = new OwnerEntity
                {
                    FirstName = createOwnerDto.FirstName,
                    LastName = createOwnerDto.LastName,
                    Email = createOwnerDto.Email,
                    PhoneNumber = createOwnerDto.PhoneNumber,
                    Address = createOwnerDto.Address,
                    City = createOwnerDto.City
                };

                var createdOwner = await _ownerService.CreateOwnerAsync(owner);
                return CreatedAtAction(nameof(GetOwnerById), new { id = createdOwner.Id }, createdOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating owner");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing owner
        /// </summary>
        [HttpPut("UpdateOwner")]
        public async Task<ActionResult<OwnerDto>> UpdateOwner([FromBody] UpdateOwnerDto updateOwnerDto)
        {
            try
            {
                var updatedOwner = await _ownerService.UpdateOwnerAsync(updateOwnerDto);

                if (updatedOwner == null)
                    return NotFound($"Owner with ID {updateOwnerDto.Id} not found");
                return Ok(updatedOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating owner");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an owner
        /// </summary>
        [HttpDelete("DeleteOwner")]
        public async Task<IActionResult> DeleteOwner([FromQuery]int id)
        {
            try
            {
                var deleted = await _ownerService.DeleteOwnerAsync(id);
                if (!deleted)
                    return NotFound($"Owner with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting owner");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Soft deletes an owner (sets IsActive to false)
        /// Also soft deletes all their pets
        /// </summary>
        [HttpDelete("SoftDeleteOwner")]
        public async Task<IActionResult> SoftDeleteOwner([FromQuery]int id)
        {
            try
            {
                var deleted = await _ownerService.SoftDeleteOwnerAsync(id);
                if (!deleted)
                    return NotFound($"Owner with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting owner");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}

