using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the MedicalHistory entity
    /// </summary>
    public class MedicalHistoryRepository(DogVetContext context) : Repository<MedicalHistoryEntity>(context), IMedicalHistoryRepository
    {
        public override async Task<MedicalHistoryEntity?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Pet)
                .Include(m => m.FollowUpOfRecord)
                .Include(m => m.Prescriptions)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MedicalHistoryEntity?> GetHistoryWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Pet)
                .Include(m => m.Veterinarian)
                .Include(m => m.FollowUpOfRecord)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MedicalHistoryEntity>> GetAllActiveAsync()
        {
            return await _dbSet.Where(m => m.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<MedicalHistoryEntity>> GetByPetIdAsync(int petId)
        {
            return await _dbSet
                .Where(m => m.PetId == petId && m.IsActive)
                .OrderByDescending(m => m.VisitDate)
                .ToListAsync();
        }
    }
}

