using DogVetAPI.Data;
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
