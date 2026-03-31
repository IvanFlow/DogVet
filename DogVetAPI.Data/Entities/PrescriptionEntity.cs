using DogVetAPI.Data.Entities.Enums;

namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a medical prescription associated with a clinical history record
    /// </summary>
    public class PrescriptionEntity : AuditEntity
    {
        public string MedName { get; set; } = string.Empty;
        public DoseFrequency Dose { get; set; }
        public int DurationInDays { get; set; }
        public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Prescribed;

        // Foreign key
        public int MedicalHistoryId { get; set; }

        // Relationship
        public MedicalHistoryEntity? MedicalHistory { get; set; }
    }
}

