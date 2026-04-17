using DogVetAPI.Application.Application;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities.Enums;
using DogVetAPI.Data.Repositories.Interfaces;

namespace DogVetAPI.Application.Services;

public class DashboardService(
    IMedicalHistoryRepository medicalHistoryRepository,
    IOwnerRepository ownerRepository,
    IPetRepository petRepository,
    ISalesNoteRepository salesNoteRepository) : IDashboardService
{
    private readonly IMedicalHistoryRepository _medicalHistoryRepository = medicalHistoryRepository ?? throw new ArgumentNullException(nameof(medicalHistoryRepository));
    private readonly IOwnerRepository _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));
    private readonly IPetRepository _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
    private readonly ISalesNoteRepository _salesNoteRepository = salesNoteRepository ?? throw new ArgumentNullException(nameof(salesNoteRepository));

    public async Task<DashboardDto> GetDashboardKpisAsync()
    {
        var now = DateTime.UtcNow;
        var in30 = now.AddDays(30);
        var ago30 = now.AddDays(-30);

        return new DashboardDto
        {
            TotalMedicalRecords = await _medicalHistoryRepository.CountAsync(),
            RecentMedicalRecords = await _medicalHistoryRepository.CountAsync(r => r.VisitDate >= ago30),
            UpcomingFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate >= now &&
                r.FollowUpDate <= in30 &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet!.IsActive),
            MissedFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate > now &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet!.IsActive ),
            OverdueFollowUps = await _medicalHistoryRepository.CountAsync(r =>
                r.FollowUpDate.HasValue &&
                r.FollowUpDate < now &&
                r.Status != MedicalHistoryStatusStrings.Completed &&
                r.Pet!.IsActive ),
            TotalOwners = await _ownerRepository.CountAsync(),
            TotalPets = await _petRepository.CountAsync(),
            RecentSaleNotes = await _salesNoteRepository.CountAsync(n => n.NoteDate >= ago30),
            PendingSaleNotes = await _salesNoteRepository.CountAsync(n => n.PaymentStatus == PaymentStatus.Pending)
        };
    }
}
