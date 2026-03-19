namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Entity representing a pet owner
    /// </summary>
    public class Owner : AuditEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        // One-to-many relationships
        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
