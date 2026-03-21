using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the Pet entity
    /// </summary>
    public class PetRepository : Repository<Pet>, IPetRepository
    {
        public PetRepository(DogVetContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Pet>> GetPetsByOwnerAsync(int ownerId)
        {
            return await _dbSet.Where(p => p.OwnerId == ownerId).ToListAsync();
        }

        public async Task<Pet?> GetPetWithHistoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.MedicalHistories)
                .ThenInclude(m => m.Veterinarian)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> GetAllActiveAsync()
        {
            return await _dbSet.Where(p => p.IsActive).ToListAsync();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            // Load pet with appointments so EF's ClientCascade can delete them
            // before the DB delete fires (Pet→Appointments uses ClientCascade)
            var pet = await _dbSet
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
                return false;

            _dbSet.Remove(pet);
            return true;
        }
    }
}
