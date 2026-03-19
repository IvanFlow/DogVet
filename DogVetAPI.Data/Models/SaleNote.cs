using DogVetAPI.Data.Models.Enums;

namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing a sale note associated with a clinical history record
    /// </summary>
    public class SaleNote : AuditEntity
    {
        public DateTime NoteDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Foreign key
        public int MedicalHistoryId { get; set; }

        // Relationships
        public MedicalHistory? MedicalHistory { get; set; }
        public ICollection<SaleNoteConcept> Concepts { get; set; } = new List<SaleNoteConcept>();
    }
}
