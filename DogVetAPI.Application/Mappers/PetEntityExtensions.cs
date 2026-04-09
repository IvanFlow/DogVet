using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Mappers
{
    /// <summary>
    /// Extension methods for mapping PetEntity to PetDto
    /// </summary>
    public static class PetEntityExtensions
    {
        public static PetDto? ToDto(this PetEntity? entity, bool withHistory = false)
        {
            if (entity == null)
                return null;

            var dto = new PetDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Breed = entity.Breed,
                Weight = entity.Weight,
                Color = entity.Color,
                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth,
                Species = entity.Species,
                IsActive = entity.IsActive,
                OwnerId = entity.OwnerId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                MedicalHistories = withHistory && entity.MedicalHistories != null
                    ? entity.MedicalHistories.Select(m => m.ToDto()!).ToList()
                    : null
            };

            return dto;
        }

        public static IEnumerable<PetDto> ToDto(this IEnumerable<PetEntity> entities)
        {
            return entities?.Select(e => e.ToDto()!) ?? Enumerable.Empty<PetDto>();
        }
    }
}
