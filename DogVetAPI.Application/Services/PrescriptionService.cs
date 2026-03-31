using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Application.Mappers;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Application;

namespace DogVetAPI.Application.Services
{
    /// <summary>
    /// Business logic service for prescriptions
    /// </summary>
    public class PrescriptionService(IPrescriptionRepository prescriptionRepository, IRepository<MedicalHistoryEntity> medicalHistoryRepository) : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository = prescriptionRepository ?? throw new ArgumentNullException(nameof(prescriptionRepository));
        private readonly IRepository<MedicalHistoryEntity> _medicalHistoryRepository = medicalHistoryRepository ?? throw new ArgumentNullException(nameof(medicalHistoryRepository));

        public async Task<IEnumerable<PrescriptionDto>> GetByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var prescriptions = await _prescriptionRepository.GetByMedicalHistoryIdAsync(medicalHistoryId);
            return prescriptions.ToDto();
        }

        public async Task<IEnumerable<PrescriptionDto>> CreateOrUpdatePrescriptionsAsync(CreatePrescriptionsRequest request)
        {
            // Verify that the medical history record exists
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(request.MedicalHistoryId);
            if (medicalHistory == null)
                throw new InvalidOperationException($"Medical history record with ID {request.MedicalHistoryId} not found");

            // Delete existing prescriptions
            await _prescriptionRepository.DeleteByMedicalHistoryIdAsync(request.MedicalHistoryId);

            // Convert request items to Prescription entities
            var prescriptions = request.Prescriptions.Select(p => new PrescriptionEntity
            {
                MedName = p.MedName,
                Dose = Enum.Parse<DoseFrequency>(p.Dose),
                DurationInDays = p.DurationInDays,
                Status = Enum.Parse<PrescriptionStatus>(p.Status),
                MedicalHistoryId = request.MedicalHistoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList();


            var createdPrescription = await _prescriptionRepository.AddRangeAsync(prescriptions);


            await _prescriptionRepository.SaveChangesAsync();
            
            return prescriptions.Select(s => s.ToDto()).ToList();
        }

    }
}

