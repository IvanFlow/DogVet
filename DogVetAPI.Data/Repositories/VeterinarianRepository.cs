using DogVetAPI.Data;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the Veterinarian entity
    /// </summary>
    public class VeterinarianRepository : Repository<Veterinarian>, IVeterinarianRepository
    {
        public VeterinarianRepository(DogVetContext context) : base(context)
        {
        }

        public async Task<Veterinarian?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.Email == email);
        }

        public async Task<IEnumerable<Veterinarian>> GetActiveVeterinariansAsync()
        {
            return await _dbSet.Where(v => v.IsActive).ToListAsync();
        }
    }
}
