namespace DogVetAPI.Application
{
    /// <summary>
    /// Request model for creating prescriptions
    /// </summary>
    public class CreatePrescriptionsRequest
    {
        public int MedicalHistoryId { get; set; }
        public List<CreatePrescriptionItemRequest>? Prescriptions { get; set; }
    }

    /// <summary>
    /// Individual prescription item for creation
    /// </summary>
    public class CreatePrescriptionItemRequest
    {
        public string MedName { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public int DurationInDays { get; set; }
        public string Status { get; set; } = "Prescribed";
    }
}