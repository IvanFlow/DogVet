namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing a veterinarian
    /// </summary>
    public class Veterinarian : AuditEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // One-to-many relationship
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
    }
}
