using ArgrosHotel.Infrastructure.Repositories;
using ArgrosHotel.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias: repositorios
builder.Services.AddSingleton<IUserRepository>(provider =>
    new InMemoryUserRepository(Path.Combine("Infrastructure", "Data", "users.json")));

builder.Services.AddSingleton<IBookingRepository>(provider =>
    new InMemoryBookingRepository(Path.Combine("Infrastructure", "Data", "bookings.json")));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear endpoints para usuarios, reservas, etc
// Por ejemplo: GET /users, GET /bookings, POST /booking, etc

app.Run();