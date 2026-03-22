using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
        {
            try
            {
                var owners = await _ownerService.GetAllOwnersAsync();
                var ownerDtos = owners.Select(o => MapToDto(o)).ToList();
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
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetOwnerById(int id)
        {
            try
            {
                var owner = await _ownerService.GetOwnerByIdAsync(id);
                if (owner == null)
                    return NotFound($"Propietario con ID {id} no encontrado");

                return Ok(MapToDto(owner));
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
        [HttpGet("{id}/with-pets")]
        public async Task<ActionResult<OwnerDto>> GetOwnerWithPets(int id)
        {
            try
            {
                var owner = await _ownerService.GetOwnerWithPetsAsync(id);
                if (owner == null)
                    return NotFound($"Owner with ID {id} not found");

                return Ok(MapToDto(owner, withPets: true));
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
        [HttpPost]
        public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] CreateOwnerDto createOwnerDto)
        {
            try
            {
                var owner = new Owner
                {
                    FirstName = createOwnerDto.FirstName,
                    LastName = createOwnerDto.LastName,
                    Email = createOwnerDto.Email,
                    PhoneNumber = createOwnerDto.PhoneNumber,
                    Address = createOwnerDto.Address,
                    City = createOwnerDto.City
                };

                var createdOwner = await _ownerService.CreateOwnerAsync(owner);
                return CreatedAtAction(nameof(GetOwnerById), new { id = createdOwner.Id }, MapToDto(createdOwner));
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
        [HttpPut("{id}")]
        public async Task<ActionResult<OwnerDto>> UpdateOwner(int id, [FromBody] UpdateOwnerDto updateOwnerDto)
        {
            try
            {
                var existingOwner = await _ownerService.GetOwnerByIdAsync(id);
                if (existingOwner == null)
                    return NotFound($"Owner with ID {id} not found");

                existingOwner.FirstName = updateOwnerDto.FirstName;
                existingOwner.LastName = updateOwnerDto.LastName;
                existingOwner.Email = updateOwnerDto.Email;
                existingOwner.PhoneNumber = updateOwnerDto.PhoneNumber;
                existingOwner.Address = updateOwnerDto.Address;
                existingOwner.City = updateOwnerDto.City;

                var updatedOwner = await _ownerService.UpdateOwnerAsync(existingOwner);
                return Ok(MapToDto(updatedOwner));
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(int id)
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
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteOwner(int id)
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

        private OwnerDto MapToDto(Owner owner, bool withPets = false)
        {
            return new OwnerDto
            {
                Id = owner.Id,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                PhoneNumber = owner.PhoneNumber,
                Address = owner.Address,
                City = owner.City,
                CreatedAt = owner.CreatedAt,
                UpdatedAt = owner.UpdatedAt,
                Pets = withPets && owner.Pets != null
                    ? owner.Pets.Select(p => new PetDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Breed = p.Breed,
                        Age = p.Age,
                        Weight = p.Weight,
                        Color = p.Color,
                        Gender = p.Gender,
                        DateOfBirth = p.DateOfBirth,
                        IsActive = p.IsActive,
                        OwnerId = p.OwnerId,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    }).ToList()
                    : null
            };
        }
    }
}
