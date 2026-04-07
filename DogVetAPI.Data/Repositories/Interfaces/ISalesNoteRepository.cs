using DogVetAPI.Data.Entities;

namespace DogVetAPI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for sales notes
    /// </summary>
    public interface ISalesNoteRepository : IRepository<SaleNoteEntity>
    {
        Task<SaleNoteEntity?> GetByIdWithConceptsAsync(int id);
    }
}
