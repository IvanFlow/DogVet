using DogVetAPI.Application.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities.Enums;
using DogVetAPI.Data.Repositories.Interfaces;

namespace DogVetAPI.Application.Services;

public class DashboardService(IMedicalHistoryRepository medicalHistoryRepository) : IDashboardService
{
    private readonly IMedicalHistoryRepository _medicalHistoryRepository = medicalHistoryRepository ?? throw new ArgumentNullException(nameof(medicalHistoryRepository));

    public async Task<DashboardDto> GetDashboardKpisAsync()
    {
        var now = DateTime.UtcNow;
        var in30 = now.AddDays(30);

        return new DashboardDto
        {
            TotalMedicalRecords = await _medicalHistoryRepository.CountAsync(),
            UpcomingFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate >= now &&
                r.FollowUpDate <= in30 &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet.IsActive ),
            MissedFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate > now &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet.IsActive ),
            OverdueFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate < now &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet.IsActive )
        };
    }
}
