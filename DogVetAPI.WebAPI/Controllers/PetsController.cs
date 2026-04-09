using DogVetAPI.Application;
using DogVetAPI.Application.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities.Enums;
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
                var petDtos = await _petService.GetAllPetsAsync();
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

                return Ok(pet);
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
        public ActionResult<IEnumerable<EnumOptionDto>> GetPetSpecies()
        {
            try
            {
                return Ok(_petService.GetSpeciesOptions());
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

                return Ok(pet);
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
             
                var createdPet = await _petService.CreatePetAsync(createPetDto);
                return CreatedAtAction(nameof(GetPetById), new { id = createdPet.Id }, createdPet);
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

                var updatedPet = await _petService.UpdatePetAsync(updatePetDto);
                if (updatedPet == null) 
                    return NotFound($"Pet with ID {updatePetDto.Id} not found");

               
                return Ok(updatedPet);
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


    }
}

