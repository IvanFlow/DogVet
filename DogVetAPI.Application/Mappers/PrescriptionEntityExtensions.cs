using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Mappers
{
    /// <summary>
    /// Extension methods for mapping PrescriptionEntity to PrescriptionDto
    /// </summary>
    public static class PrescriptionEntityExtensions
    {
        public static PrescriptionDto? ToDto(this PrescriptionEntity? entity)
        {
            if (entity == null)
                return null;

            return new PrescriptionDto
            {
                Id = entity.Id,
                MedName = entity.MedName,
                Dose = entity.Dose.ToString(),
                DurationInDays = entity.DurationInDays,
                Status = entity.Status.ToString(),
                MedicalHistoryId = entity.MedicalHistoryId
            };
        }

        public static IEnumerable<PrescriptionDto> ToDto(this IEnumerable<PrescriptionEntity> entities)
        {
            return entities?.Select(e => e.ToDto()) ?? Enumerable.Empty<PrescriptionDto>();
        }
    }
}
