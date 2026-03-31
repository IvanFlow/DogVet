using DogVetAPI.Application;
using DogVetAPI.Data.Models;

namespace DogVetAPI.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for prescriptions
    /// </summary>
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<IEnumerable<PrescriptionDto>> CreateOrUpdatePrescriptionsAsync(int medicalHistoryId, IEnumerable<Prescription> prescriptions);
    }
}
