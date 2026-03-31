namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing the clinical history of a pet
    /// </summary>
    public class MedicalHistoryEntity : AuditEntity
    {
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Status { get; set; } = "Completed"; // Completed, Pending, Scheduled
        public bool IsActive { get; set; } = true;

        // Foreign keys
        public int PetId { get; set; }
        public int? VeterinarianId { get; set; }
        public int? FollowUpOf { get; set; }

        // Relationships
        public PetEntity? Pet { get; set; }
        public VeterinarianEntity? Veterinarian { get; set; }
        public MedicalHistoryEntity? FollowUpOfRecord { get; set; }
        public MedicalHistoryEntity? FollowUpRecord { get; set; }
        public ICollection<PrescriptionEntity> Prescriptions { get; set; } = new List<PrescriptionEntity>();
        public ICollection<SaleNoteEntity> SaleNotes { get; set; } = new List<SaleNoteEntity>();
    }
}

