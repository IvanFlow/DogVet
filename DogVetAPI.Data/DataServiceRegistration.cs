using DogVetAPI.Data.DBContext;
using DogVetAPI.Data.Repositories;
using DogVetAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogVetAPI.Data
{
    /// <summary>
    /// Extension methods for registering Data layer services
    /// </summary>
    public static class DataServiceRegistration
    {
        /// <summary>
        /// Register DbContext and Repository services
        /// </summary>
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
        // Configure DbContext based on environment
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Fallback: use LocalDB if no connection string is provided
            connectionString = "Server=(localdb)\\\\mssqllocaldb;Database=DogVetAPI;Trusted_Connection=true;TrustServerCertificate=true;";
        }

        services.AddDbContext<DogVetContext>(options =>
                options.UseSqlServer(connectionString));

            // Register Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register Specific Repositories
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

            return services;
        }
    }
}
