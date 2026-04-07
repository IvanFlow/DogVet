using DogVetAPI.Application.Application;
using DogVetAPI.Data.Entities;

namespace DogVetAPI.Application.Mappers;

public static class SaleNoteEntityExtensions
{
    public static SaleNoteDto ToDto(this SaleNoteEntity entity)
    {
        return new SaleNoteDto
        {
            Id = entity.Id,
            MedicalHistoryId = entity.MedicalHistoryId,
            NoteDate = entity.NoteDate,
            TotalAmount = entity.TotalAmount,
            PaymentStatus = entity.PaymentStatus.ToString(),
            Concepts = entity.Concepts?.Select(c => c.ToDto()).ToList() ?? new()
        };
    }

    public static SaleNoteConceptDto ToDto(this SaleNoteConceptEntity entity)
    {
        return new SaleNoteConceptDto
        {
            Id = entity.Id,
            Description = entity.Description,
            Quantity = entity.Quantity,
            UnitPrice = entity.UnitPrice,
            ConceptPrice = entity.ConceptPrice
        };
    }

    public static IEnumerable<SaleNoteDto> ToDtos(this IEnumerable<SaleNoteEntity> entities)
    {
        return entities.Select(e => e.ToDto());
    }
}
