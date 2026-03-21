namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing the clinical history of a pet
    /// </summary>
    public class MedicalHistory : AuditEntity
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

        // Relationships
        public Pet? Pet { get; set; }
        public Veterinarian? Veterinarian { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<SaleNote> SaleNotes { get; set; } = new List<SaleNote>();
    }
}
