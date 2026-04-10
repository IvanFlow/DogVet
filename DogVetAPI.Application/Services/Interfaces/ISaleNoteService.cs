using DogVetAPI.Application.Application;

namespace DogVetAPI.Application.Services.Interfaces;

public interface ISaleNoteService
{
    Task<IEnumerable<SaleNoteDto>> GetAllAsync();
    Task<SaleNoteDto> CreateAsync(CreateSaleNoteRequest request);
    Task<SaleNoteDto?> GetByIdAsync(int id);
    Task<IEnumerable<SaleNoteDto>> GetByMedicalHistoryIdAsync(int medicalHistoryId);
    Task<bool> DeleteAsync(int id);
    Task<SaleNoteDto?> UpdatePaymentStatusAsync(int id, string paymentStatus);
    IEnumerable<EnumOptionDto> GetPaymentStatusOptions();
}
