using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the Owner entity
    /// </summary>
    public class OwnerRepository(DogVetContext context) : Repository<OwnerEntity>(context), IOwnerRepository
    {
        public async Task<OwnerEntity?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<IEnumerable<OwnerEntity>> GetOwnersWithPetsAsync()
        {
            return await _dbSet.Include(o => o.Pets).ToListAsync();
        }

        public async Task<OwnerEntity?> GetOwnerWithPetsAsync(int id)
        {
            return await _dbSet.Include(o => o.Pets).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OwnerEntity?> GetOwnerWithPetsWithMedicalHistoriesAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Pets)
                    .ThenInclude(p => p.MedicalHistories)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OwnerEntity>> GetAllActiveAsync()
        {
            return await _dbSet.Where(o => o.IsActive).ToListAsync();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            // Load owner with pets and their appointments so EF's ClientCascade
            // can delete pet appointments before the DB cascade fires on pets
            var owner = await _dbSet
                .Include(o => o.Pets)
                    .ThenInclude(p => p.Appointments)
                .Include(o => o.Appointments)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null)
                return false;

            _dbSet.Remove(owner);
            return true;
        }
    }
}

