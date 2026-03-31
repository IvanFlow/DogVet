using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Models;
using DogVetAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for prescription entities
    /// </summary>
    public class PrescriptionRepository(DogVetContext context) : Repository<Prescription>(context), IPrescriptionRepository
    {
        public async Task<IEnumerable<Prescription>> GetByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            return await _dbSet
                .Where(p => p.MedicalHistoryId == medicalHistoryId)
                .ToListAsync();
        }

        public async Task<bool> DeleteByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var prescriptions = await _dbSet
                .Where(p => p.MedicalHistoryId == medicalHistoryId)
                .ToListAsync();

            if (!prescriptions.Any())
                return true; // No prescriptions to delete

            _dbSet.RemoveRange(prescriptions);
            await SaveChangesAsync();
            return true;
        }
    }
}
