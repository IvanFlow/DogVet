using DogVetAPI.Data.Entities.Enums;

namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a sale note associated with a clinical history record
    /// </summary>
    public class SaleNoteEntity : AuditEntity
    {
        public DateTime NoteDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Foreign key
        public int MedicalHistoryId { get; set; }

        // Relationships
        public MedicalHistoryEntity? MedicalHistory { get; set; }
        public ICollection<SaleNoteConceptEntity> Concepts { get; set; } = new List<SaleNoteConceptEntity>();
    }
}

