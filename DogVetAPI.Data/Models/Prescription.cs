using DogVetAPI.Data.Models.Enums;

namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing a medical prescription associated with a clinical history record
    /// </summary>
    public class Prescription : AuditEntity
    {
        public string MedName { get; set; } = string.Empty;
        public DoseFrequency Dose { get; set; }
        public int DurationInDays { get; set; }
        public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Prescribed;

        // Foreign key
        public int MedicalHistoryId { get; set; }

        // Relationship
        public MedicalHistory? MedicalHistory { get; set; }
    }
}
