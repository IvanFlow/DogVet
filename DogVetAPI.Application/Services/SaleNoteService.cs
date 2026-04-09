using DogVetAPI.Application.Application;
using DogVetAPI.Application.Mappers;
using DogVetAPI.Application.Services.Interfaces;
using DogVetAPI.Data.Entities;
using DogVetAPI.Data.Entities.Enums;
using DogVetAPI.Data.Repositories.Interfaces;

namespace DogVetAPI.Application.Services;

public class SaleNoteService : ISaleNoteService
{
    private readonly ISalesNoteRepository _saleNoteRepository;
    private readonly IRepository<SaleNoteConceptEntity> _conceptRepository;
    private readonly IPrescriptionService _prescriptionService;

    public SaleNoteService(
        ISalesNoteRepository saleNoteRepository,
        IRepository<SaleNoteConceptEntity> conceptRepository,
        IPrescriptionService prescriptionService)
    {
        _saleNoteRepository = saleNoteRepository;
        _conceptRepository = conceptRepository;
        _prescriptionService = prescriptionService;
    }

    public async Task<SaleNoteDto> CreateAsync(CreateSaleNoteRequest request)
    {
        var saleNote = new SaleNoteEntity
        {
            MedicalHistoryId = request.MedicalHistoryId,
            NoteDate = DateTime.UtcNow,
            TotalAmount = request.Concepts.Sum(c => c.ConceptPrice),
            PaymentStatus = PaymentStatus.Pending
        };

        await _saleNoteRepository.AddAsync(saleNote);
        await _saleNoteRepository.SaveChangesAsync();

        // Add concepts
        foreach (var conceptDto in request.Concepts)
        {
            var concept = new SaleNoteConceptEntity
            {
                SaleNoteId = saleNote.Id,
                Description = conceptDto.Description,
                Quantity = conceptDto.Quantity,
                UnitPrice = conceptDto.UnitPrice,
                ConceptPrice = conceptDto.ConceptPrice
            };

            await _conceptRepository.AddAsync(concept);
        }

        await _conceptRepository.SaveChangesAsync();

        // Update prescription statuses to Administered
        if (request.PrescriptionIds != null && request.PrescriptionIds.Count > 0)
        {
            foreach (var prescriptionId in request.PrescriptionIds)
            {
                await _prescriptionService.UpdatePrescriptionStatusAsync(prescriptionId, PrescriptionStatus.Administered.ToString());
            }
        }

        return saleNote.ToDto();
    }

    public async Task<SaleNoteDto?> GetByIdAsync(int id)
    {
        var saleNote = await _saleNoteRepository.GetByIdWithConceptsAsync(id);
        return saleNote?.ToDto();
    }

    public async Task<IEnumerable<SaleNoteDto>> GetByMedicalHistoryIdAsync(int medicalHistoryId)
    {
        var allNotes = await _saleNoteRepository.GetAllAsync();
        var notes = allNotes.Where(sn => sn.MedicalHistoryId == medicalHistoryId);
        return notes.ToDtos();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var saleNote = await _saleNoteRepository.GetByIdAsync(id);
            if (saleNote == null)
                return false;

            await _saleNoteRepository.DeleteAsync(id);
            await _saleNoteRepository.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<SaleNoteDto?> UpdatePaymentStatusAsync(int id, string paymentStatus)
    {
        if (!Enum.TryParse<PaymentStatus>(paymentStatus, out var status))
            throw new ArgumentException($"Estado de pago inválido: {paymentStatus}");

        var saleNote = await _saleNoteRepository.GetByIdAsync(id);
        if (saleNote == null)
            return null;

        saleNote.PaymentStatus = status;
        await _saleNoteRepository.SaveChangesAsync();
        return saleNote.ToDto();
    }

    public IEnumerable<EnumOptionDto> GetPaymentStatusOptions()
    {
        return Enum.GetValues(typeof(PaymentStatus))
            .Cast<PaymentStatus>()
            .OrderBy(s => (int)s)
            .Select(s => new EnumOptionDto { Value = s.ToString(), Id = (int)s });
    }
}
