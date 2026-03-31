using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Repositories.Interfaces;

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
            return prescriptions.Select(p => MapToDto(p));
        }

        public async Task<IEnumerable<PrescriptionDto>> CreateOrUpdatePrescriptionsAsync(int medicalHistoryId, IEnumerable<PrescriptionEntity> prescriptions)
        {
            // Verify that the medical history record exists
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId);
            if (medicalHistory == null)
                throw new InvalidOperationException($"Medical history record with ID {medicalHistoryId} not found");

            // Delete existing prescriptions
            await _prescriptionRepository.DeleteByMedicalHistoryIdAsync(medicalHistoryId);

            // Create new prescriptions
            var newPrescriptions = new List<PrescriptionEntity>();
            foreach (var prescription in prescriptions)
            {
                prescription.MedicalHistoryId = medicalHistoryId;
                prescription.CreatedAt = DateTime.UtcNow;
                prescription.UpdatedAt = DateTime.UtcNow;

                var createdPrescription = await _prescriptionRepository.AddAsync(prescription);
                newPrescriptions.Add(createdPrescription);
            }

            await _prescriptionRepository.SaveChangesAsync();
            
            return newPrescriptions.Select(p => MapToDto(p));
        }

        private PrescriptionDto MapToDto(PrescriptionEntity prescription)
        {
            return new PrescriptionDto
            {
                Id = prescription.Id,
                MedName = prescription.MedName,
                Dose = prescription.Dose.ToString(),
                DurationInDays = prescription.DurationInDays,
                Status = prescription.Status.ToString(),
                MedicalHistoryId = prescription.MedicalHistoryId
            };
        }
    }
}

