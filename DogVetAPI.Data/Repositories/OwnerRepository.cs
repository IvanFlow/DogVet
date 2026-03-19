using DogVetAPI.Data;
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
    }
}
