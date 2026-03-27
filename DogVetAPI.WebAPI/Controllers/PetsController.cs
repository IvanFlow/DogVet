using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;
using DogVetAPI.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController(IPetService petService, ILogger<PetsController> logger) : ControllerBase
    {
        private readonly IPetService _petService = petService ?? throw new ArgumentNullException(nameof(petService));
        private readonly ILogger<PetsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets all pets
        /// </summary>
        [HttpGet("GetAllPets")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets()
        {
            try
            {
                var pets = await _petService.GetAllPetsAsync();
                var petDtos = pets.Select(p => MapToDto(p)).ToList();
                return Ok(petDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pets");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a pet by ID
        /// </summary>
        [HttpGet("GetPetById")]
        public async Task<ActionResult<PetDto>> GetPetById([FromQuery]int id)
        {
            try
            {
                var pet = await _petService.GetPetByIdAsync(id);
                if (pet == null)
                    return NotFound($"Mascota con ID {id} no encontrada");

                return Ok(MapToDto(pet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all available species values
        /// </summary>
        [HttpGet("GetPetSpecies")]
        public ActionResult<IEnumerable<object>> GetPetSpecies()
        {
            try
            {
                var species = Enum.GetValues(typeof(Species))
                    .Cast<Species>()
                    .Select(s => new { value = s.ToString(), id = (int)s })
                    .OrderBy(x => x.id)
                    .ToList();

                return Ok(species);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving species");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a pet with its medical history
        /// </summary>
        [HttpGet("GetPetWithHistory")]
        public async Task<ActionResult<PetDto>> GetPetWithHistory([FromQuery]int id)
        {
            try
            {
                var pet = await _petService.GetPetWithHistoryAsync(id);
                if (pet == null)
                    return NotFound($"Mascota con ID {id} no encontrada");

                return Ok(MapToDto(pet, withHistory: true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet with history");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new pet
        /// </summary>
        [HttpPost("CreatePet")]
        public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetDto createPetDto)
        {
            try
            {
                // Validate species if provided
                if (!string.IsNullOrEmpty(createPetDto.Species) && !SpeciesExtensions.IsValidSpecies(createPetDto.Species))
                {
                    return BadRequest($"Invalid species: {createPetDto.Species}");
                }

                var pet = new Pet
                {
                    Name = createPetDto.Name,
                    Breed = createPetDto.Breed,
                    Weight = createPetDto.Weight,
                    Color = createPetDto.Color,
                    Gender = createPetDto.Gender,
                    DateOfBirth = createPetDto.DateOfBirth,
                    Species = createPetDto.Species,
                    OwnerId = createPetDto.OwnerId
                };

                var createdPet = await _petService.CreatePetAsync(pet);
                return CreatedAtAction(nameof(GetPetById), new { id = createdPet.Id }, MapToDto(createdPet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pet");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing pet
        /// </summary>
        [HttpPut("UpdatePet")]
        public async Task<ActionResult<PetDto>> UpdatePet([FromBody] UpdatePetDto updatePetDto)
        {
            try
            {
                // Validate species if provided
                if (!string.IsNullOrEmpty(updatePetDto.Species) && !SpeciesExtensions.IsValidSpecies(updatePetDto.Species))
                {
                    return BadRequest($"Invalid species: {updatePetDto.Species}");
                }

                var existingPet = await _petService.GetPetByIdAsync(updatePetDto.Id);
                if (existingPet == null)
                    return NotFound($"Pet with ID {updatePetDto.Id} not found");

                existingPet.Name = updatePetDto.Name;
                existingPet.Breed = updatePetDto.Breed;
                existingPet.Weight = updatePetDto.Weight;
                existingPet.Color = updatePetDto.Color;
                existingPet.Gender = updatePetDto.Gender;
                existingPet.DateOfBirth = updatePetDto.DateOfBirth;
                existingPet.Species = updatePetDto.Species;
                existingPet.IsActive = updatePetDto.IsActive;
                existingPet.OwnerId = updatePetDto.OwnerId;

                var updatedPet = await _petService.UpdatePetAsync(existingPet);
                return Ok(MapToDto(updatedPet));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pet");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a pet
        /// </summary>
        [HttpDelete("DeletePet")]
        public async Task<IActionResult> DeletePet([FromQuery]int id)
        {
            try
            {
                var deleted = await _petService.DeletePetAsync(id);
                if (!deleted)
                    return NotFound($"Pet with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pet");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Soft deletes a pet (sets IsActive to false)
        /// Also soft deletes all their medical histories
        /// </summary>
        [HttpDelete("SoftDeletePet")]
        public async Task<IActionResult> SoftDeletePet([FromQuery]int id)
        {
            try
            {
                var deleted = await _petService.SoftDeletePetAsync(id);
                if (!deleted)
                    return NotFound($"Pet with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting pet");
                return StatusCode(500, "Internal server error");
            }
        }

        private PetDto MapToDto(Pet pet, bool withHistory = false)
        {
            return new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Breed = pet.Breed,
                Weight = pet.Weight,
                Color = pet.Color,
                Gender = pet.Gender,
                DateOfBirth = pet.DateOfBirth,
                Species = pet.Species,
                IsActive = pet.IsActive,
                OwnerId = pet.OwnerId,
                CreatedAt = pet.CreatedAt,
                UpdatedAt = pet.UpdatedAt,
                MedicalHistories = withHistory && pet.MedicalHistories != null
                    ? pet.MedicalHistories.Select(m => new MedicalHistoryDto
                    {
                        Id = m.Id,
                        Diagnosis = m.Diagnosis,
                        Notes = m.Notes,
                        VisitDate = m.VisitDate,
                        FollowUpDate = m.FollowUpDate,
                        Status = m.Status,
                        PetId = m.PetId,
                        VeterinarianId = m.VeterinarianId
                    }).ToList()
                    : null
            };
        }
    }
}
