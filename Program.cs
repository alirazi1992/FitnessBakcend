// Program.cs
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FitnessBakcend.Data;

var builder = WebApplication.CreateBuilder(args);

// Connection string (throws early if missing)
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? throw new InvalidOperationException("Missing 'DefaultConnection' in appsettings.json");

// DbContext (Pomelo MySQL)
builder.Services.AddDbContext<FitnessContext>(o =>
    o.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// Controllers + serialize enums as strings
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// CORS for Next.js (http://localhost:3000)
builder.Services.AddCors(o =>
    o.AddPolicy("Frontend", p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger (dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
