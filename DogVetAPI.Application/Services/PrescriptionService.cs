using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Application.Application;
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

            // If no prescriptions provided, return empty list
            if (request.Prescriptions == null || request.Prescriptions.Count == 0)
            {
                await _prescriptionRepository.SaveChangesAsync();
                return new List<PrescriptionDto>();
            }

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

            await _prescriptionRepository.AddRangeAsync(prescriptions);
            await _prescriptionRepository.SaveChangesAsync();
            
            return prescriptions.ToDto() ?? new List<PrescriptionDto>();
        }

        public async Task<bool> DeleteAllPrescriptionsByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            try
            {
                await _prescriptionRepository.DeleteByMedicalHistoryIdAsync(medicalHistoryId);
                await _prescriptionRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePrescriptionStatusAsync(int prescriptionId, string status)
        {
            try
            {
                if (!Enum.TryParse<PrescriptionStatus>(status, out var prescriptionStatus))
                    throw new ArgumentException($"Invalid prescription status: {status}");

                var prescription = await _prescriptionRepository.GetByIdAsync(prescriptionId);
                if (prescription == null)
                    return false;

                prescription.Status = prescriptionStatus;
                prescription.UpdatedAt = DateTime.UtcNow;
                await _prescriptionRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<EnumOptionDto> GetDoseFrequencyOptions()
        {
            return Enum.GetValues(typeof(DoseFrequency))
                .Cast<DoseFrequency>()
                .OrderBy(d => (int)d)
                .Select(d => new EnumOptionDto { Value = d.ToString(), Id = (int)d });
        }

        public IEnumerable<EnumOptionDto> GetPrescriptionStatusOptions()
        {
            return Enum.GetValues(typeof(PrescriptionStatus))
                .Cast<PrescriptionStatus>()
                .OrderBy(s => (int)s)
                .Select(s => new EnumOptionDto { Value = s.ToString(), Id = (int)s });
        }
    }
}

