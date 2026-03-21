using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the MedicalHistory entity
    /// </summary>
    public class MedicalHistoryRepository(DogVetContext context) : Repository<MedicalHistory>(context), IMedicalHistoryRepository
    {
        public async Task<MedicalHistory?> GetHistoryWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Pet)
                .Include(m => m.Veterinarian)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<MedicalHistory>> GetAllActiveAsync()
        {
            return await _dbSet.Where(m => m.IsActive).ToListAsync();
        }
    }
}
