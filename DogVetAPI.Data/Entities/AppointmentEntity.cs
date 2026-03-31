using DogVetAPI.Data.Entities.Enums;

namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Represents a veterinary appointment
    /// </summary>
    public class AppointmentEntity : AuditEntity
    {
        /// <summary>
        /// Date and time of the appointment
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Status of the appointment
        /// </summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        /// <summary>
        /// Foreign key for the owner
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Navigation property for the owner
        /// </summary>
        public OwnerEntity Owner { get; set; } = null!;

        /// <summary>
        /// Foreign key for the pet
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// Navigation property for the pet
        /// </summary>
        public PetEntity Pet { get; set; } = null!;
    }
}

