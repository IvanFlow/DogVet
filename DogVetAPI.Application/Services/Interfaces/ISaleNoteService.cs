using DogVetAPI.Application.Application;

namespace DogVetAPI.Application.Services.Interfaces;

public interface ISaleNoteService
{
    Task<SaleNoteDto> CreateAsync(CreateSaleNoteRequest request);
    Task<SaleNoteDto?> GetByIdAsync(int id);
    Task<IEnumerable<SaleNoteDto>> GetByMedicalHistoryIdAsync(int medicalHistoryId);
    Task<bool> DeleteAsync(int id);
}
