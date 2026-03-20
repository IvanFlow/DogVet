using DogVetAPI.Data;
using DogVetAPI.Data.DBContext;
using DogVetAPI.Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Register Data and Application services
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed local database
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DogVetContext>();
        try
        {
            await dbContext.SeedAllDataAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error al migrar o sembrar la base de datos.");
        }
    }
}

// Configure the HTTP request pipeline
app.MapOpenApi();
app.MapScalarApiReference("/api/scalar");
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Serve static files (Angular app)
app.UseStaticFiles();

// API Controllers
app.MapControllers();

// Fallback to index.html for Angular routing
app.MapFallbackToFile("index.html");

app.Run();
