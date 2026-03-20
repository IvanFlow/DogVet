using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the Owner entity
    /// </summary>
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        public OwnerRepository(DogVetContext context) : base(context)
        {
        }

        public async Task<Owner?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<IEnumerable<Owner>> GetOwnersWithPetsAsync()
        {
            return await _dbSet.Include(o => o.Pets).ToListAsync();
        }

        public async Task<Owner?> GetOwnerWithPetsAsync(int id)
        {
            return await _dbSet.Include(o => o.Pets).FirstOrDefaultAsync(o => o.Id == id);
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
