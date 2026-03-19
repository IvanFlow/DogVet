using DogVetAPI.Data;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the MedicalHistory entity
    /// </summary>
    public class MedicalHistoryRepository : Repository<MedicalHistory>, IMedicalHistoryRepository
    {
        public MedicalHistoryRepository(DogVetContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicalHistory>> GetHistoryByPetAsync(int petId)
        {
            return await _dbSet
                .Where(m => m.PetId == petId)
                .OrderByDescending(m => m.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalHistory>> GetHistoryByVeterinarianAsync(int veterinarianId)
        {
            return await _dbSet
                .Where(m => m.VeterinarianId == veterinarianId)
                .OrderByDescending(m => m.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalHistory>> GetHistoryByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(m => m.VisitDate >= startDate && m.VisitDate <= endDate)
                .OrderByDescending(m => m.VisitDate)
                .ToListAsync();
        }

        public async Task<MedicalHistory?> GetHistoryWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Pet)
                .Include(m => m.Veterinarian)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
