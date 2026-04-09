using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Mappers
{
    /// <summary>
    /// Extension methods for mapping OwnerEntity to OwnerDto
    /// </summary>
    public static class OwnerEntityExtensions
    {
        public static OwnerDto? ToDto(this OwnerEntity? entity, bool withPets = false)
        {
            if (entity == null)
                return null;

            var dto = new OwnerDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                City = entity.City,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Pets = withPets && entity.Pets != null
                    ? entity.Pets.Select(p => p.ToDto()!).ToList()
                    : null
            };

            return dto;
        }

        public static IEnumerable<OwnerDto> ToDto(this IEnumerable<OwnerEntity> entities)
        {
            return entities?.Select(e => e.ToDto()!) ?? Enumerable.Empty<OwnerDto>();
        }
    }
}
