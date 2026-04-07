using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories.Interfaces;
using DogVetAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogVetAPI.Data.Repositories
{
    /// <summary>
    /// Repository for the SaleNote entity
    /// </summary>
    public class SalesNoteRepository(DogVetContext context) : Repository<SaleNoteEntity>(context), ISalesNoteRepository
    {
        public async Task<SaleNoteEntity?> GetByIdWithConceptsAsync(int id)
        {
            return await _dbSet
                .Include(sn => sn.Concepts)
                .FirstOrDefaultAsync(sn => sn.Id == id);
        }
    }
}
