namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a veterinarian
    /// </summary>
    public class VeterinarianEntity : AuditEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // One-to-many relationship
        public ICollection<MedicalHistoryEntity> MedicalHistories { get; set; } = new List<MedicalHistoryEntity>();
    }
}

