namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing a pet
    /// </summary>
    public class Pet : AuditEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;

        // Foreign keys
        public int OwnerId { get; set; }

        // Relationships
        public Owner? Owner { get; set; }
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
