namespace DogVetAPI.Data.Entities.Enums
{
    /// <summary>
    /// Enumeration for appointment statuses
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// Appointment is scheduled
        /// </summary>
        Scheduled = 0,

        /// <summary>
        /// Appointment has been cancelled
        /// </summary>
        Cancelled = 1,

        /// <summary>
        /// Appointment has been completed
        /// </summary>
        Completed = 2
    }
}

