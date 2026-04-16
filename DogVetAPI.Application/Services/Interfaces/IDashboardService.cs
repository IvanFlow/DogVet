using DogVetAPI.Application.Application;

namespace DogVetAPI.Application.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardKpisAsync();
}
