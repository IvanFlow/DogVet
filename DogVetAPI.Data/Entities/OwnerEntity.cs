namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a pet owner
    /// </summary>
    public class OwnerEntity : AuditEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // One-to-many relationships
        public ICollection<PetEntity> Pets { get; set; } = new List<PetEntity>();
        public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
    }
}

