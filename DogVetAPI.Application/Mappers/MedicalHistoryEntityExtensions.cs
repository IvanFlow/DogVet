using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Mappers
{
    /// <summary>
    /// Extension methods for mapping MedicalHistoryEntity to MedicalHistoryDto
    /// </summary>
    public static class MedicalHistoryEntityExtensions
    {
        public static MedicalHistoryDto? ToDto(this MedicalHistoryEntity? entity, bool includeFollowUpOfRecord = false)
        {
            if (entity == null)
                return null;

            var dto = new MedicalHistoryDto
            {
                Id = entity.Id,
                Diagnosis = entity.Diagnosis,
                Notes = entity.Notes,
                VisitDate = entity.VisitDate,
                FollowUpDate = entity.FollowUpDate,
                Status = entity.Status,
                PetId = entity.PetId,
                VeterinarianId = entity.VeterinarianId,
                FollowUpOf = entity.FollowUpOf
            };

            if (entity.Pet != null)
            {
                dto.Pet = entity.Pet.ToDto();
            }

            if (entity.Prescriptions != null && entity.Prescriptions.Any())
            {
                dto.Prescriptions = entity.Prescriptions.Select(p => p.ToDto()!).ToList();
            }

            if (includeFollowUpOfRecord && entity.FollowUpOfRecord != null)
            {
                dto.FollowUpOfRecord = entity.FollowUpOfRecord.ToDto(includeFollowUpOfRecord: false);
            }

            return dto;
        }

        public static IEnumerable<MedicalHistoryDto> ToDto(this IEnumerable<MedicalHistoryEntity> entities)
        {
            return entities?.Select(e => e.ToDto()!) ?? Enumerable.Empty<MedicalHistoryDto>();
        }
    }
}
