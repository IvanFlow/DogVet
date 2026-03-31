namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a pet
    /// </summary>
    public class PetEntity : AuditEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Species { get; set; }
        public bool IsActive { get; set; } = true;

        // Foreign keys
        public int OwnerId { get; set; }

        // Relationships
        public OwnerEntity? Owner { get; set; }
        public ICollection<MedicalHistoryEntity> MedicalHistories { get; set; } = new List<MedicalHistoryEntity>();
        public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
    }
}

