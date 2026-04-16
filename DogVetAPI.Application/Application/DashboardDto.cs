namespace DogVetAPI.Application.Application;

public class DashboardDto
{
    public int TotalMedicalRecords { get; set; }
    public int UpcomingFollowUps { get; set; }
    public int MissedFollowUps { get; set; }
    public int OverdueFollowUps { get; set; }
}
