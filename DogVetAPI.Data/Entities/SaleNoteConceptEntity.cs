namespace DogVetAPI.Data.Entities
{
    /// <summary>
    /// Entity representing a billable item within a sale note
    /// </summary>
    public class SaleNoteConceptEntity : AuditEntity
    {
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ConceptPrice { get; set; }

        // Foreign key
        public int SaleNoteId { get; set; }

        // Relationship
        public SaleNoteEntity? SaleNote { get; set; }
    }
}

