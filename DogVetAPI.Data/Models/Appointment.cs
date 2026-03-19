using DogVetAPI.Data.Models.Enums;

namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Represents a veterinary appointment
    /// </summary>
    public class Appointment : AuditEntity
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
        public Owner Owner { get; set; } = null!;

        /// <summary>
        /// Foreign key for the pet
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// Navigation property for the pet
        /// </summary>
        public Pet Pet { get; set; } = null!;
    }
}
