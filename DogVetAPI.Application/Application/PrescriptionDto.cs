namespace DogVetAPI.Application
{
    /// <summary>
    /// DTO for prescription data
    /// </summary>
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string MedName { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public int DurationInDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MedicalHistoryId { get; set; }
    }
}
