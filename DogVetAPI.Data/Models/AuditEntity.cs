namespace DogVetAPI.Data.Models
{
    /// <summary>
    /// Base audit entity containing creation and update timestamp information
    /// </summary>
    public abstract class AuditEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
