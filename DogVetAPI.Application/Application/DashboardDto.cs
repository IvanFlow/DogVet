namespace DogVetAPI.Application.Application;

public class DashboardDto
{
    public int TotalMedicalRecords { get; set; }
    public int RecentMedicalRecords { get; set; }
    public int UpcomingFollowUps { get; set; }
    public int MissedFollowUps { get; set; }
    public int OverdueFollowUps { get; set; }
    public int TotalOwners { get; set; }
    public int TotalPets { get; set; }
    public int RecentSaleNotes { get; set; }
    public int PendingSaleNotes { get; set; }
}
