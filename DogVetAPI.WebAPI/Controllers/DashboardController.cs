using DogVetAPI.Application.Application;
using DogVetAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DogVetAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
    private readonly ILogger<DashboardController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Gets all dashboard KPIs
    /// </summary>
    [HttpGet("GetKpis")]
    public async Task<ActionResult<DashboardDto>> GetKpis()
    {
        try
        {
            var kpis = await _dashboardService.GetDashboardKpisAsync();
            return Ok(kpis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard KPIs");
            return StatusCode(500, "Internal server error");
        }
    }
}
