using DogVetAPI.Application.Services;
using DogVetAPI.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DogVetAPI.Application
{
    /// <summary>
    /// Extension methods for registering Application layer services
    /// </summary>
    public static class ApplicationServiceRegistration
    {
        /// <summary>
        /// Register business logic services
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();

            return services;
        }
    }
}
