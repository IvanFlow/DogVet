namespace DogVetAPI.Application.Application;

public class EnumOptionDto
{
    public string Value { get; set; } = string.Empty;
    public int Id { get; set; }
}

public class SaleNoteDto
{
    public int Id { get; set; }
    public int MedicalHistoryId { get; set; }
    public DateTime NoteDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public List<SaleNoteConceptDto> Concepts { get; set; } = new();
}

public class SaleNoteConceptDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ConceptPrice { get; set; }
}

public class CreateSaleNoteRequest
{
    public int MedicalHistoryId { get; set; }
    public List<SaleNoteConceptDto> Concepts { get; set; } = new();
    public List<int> PrescriptionIds { get; set; } = new();
}
