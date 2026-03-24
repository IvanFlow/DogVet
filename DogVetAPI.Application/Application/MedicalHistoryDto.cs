namespace DogVetAPI.Application
{
    /// <summary>
    /// DTO for medical history data
    /// </summary>
    public class MedicalHistoryDto
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int PetId { get; set; }
        public int? VeterinarianId { get; set; }
        public int? FollowUpOf { get; set; }
        public MedicalHistoryDto? FollowUpOfRecord { get; set; }
        public PetDto? Pet { get; set; }
    }

    /// <summary>
    /// DTO for creating medical history records
    /// </summary>
    public class CreateMedicalHistoryDto
    {
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int PetId { get; set; }
        public int? FollowUpOf { get; set; }
    }

    /// <summary>
    /// DTO for updating medical history records
    /// </summary>
    public class UpdateMedicalHistoryDto
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int PetId { get; set; }
        public int? VeterinarianId { get; set; }
        public int? FollowUpOf { get; set; }
    }
}
