using DogVetAPI.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Models;
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
        [HttpGet]
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
        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPetById(int id)
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
        /// Gets a pet with its medical history
        /// </summary>
        [HttpGet("with-history/{id}")]
        public async Task<ActionResult<PetDto>> GetPetWithHistory(int id)
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
        [HttpPost]
        public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetDto createPetDto)
        {
            try
            {
                var pet = new Pet
                {
                    Name = createPetDto.Name,
                    Breed = createPetDto.Breed,
                    Age = createPetDto.Age,
                    Weight = createPetDto.Weight,
                    Color = createPetDto.Color,
                    Gender = createPetDto.Gender,
                    DateOfBirth = createPetDto.DateOfBirth,
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
        [HttpPut("{id}")]
        public async Task<ActionResult<PetDto>> UpdatePet(int id, [FromBody] UpdatePetDto updatePetDto)
        {
            try
            {
                var existingPet = await _petService.GetPetByIdAsync(id);
                if (existingPet == null)
                    return NotFound($"Pet with ID {id} not found");

                existingPet.Name = updatePetDto.Name;
                existingPet.Breed = updatePetDto.Breed;
                existingPet.Age = updatePetDto.Age;
                existingPet.Weight = updatePetDto.Weight;
                existingPet.Color = updatePetDto.Color;
                existingPet.Gender = updatePetDto.Gender;
                existingPet.DateOfBirth = updatePetDto.DateOfBirth;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
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
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeletePet(int id)
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
                Age = pet.Age,
                Weight = pet.Weight,
                Color = pet.Color,
                Gender = pet.Gender,
                DateOfBirth = pet.DateOfBirth,
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
