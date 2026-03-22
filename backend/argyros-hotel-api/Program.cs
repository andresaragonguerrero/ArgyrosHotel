using argyros_hotel_api.Infrastructure.Repositories;
using argyros_hotel_api.Application.Interfaces;

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
// Endpoint de prueba: obtener todos los usuarios
app.MapGet("/users", async (IUserRepository userRepository) =>
{
    var users = await userRepository.GetAllAsync();
    return Results.Ok(users);
})
.WithName("GetAllUsers")
.WithOpenApi();

app.Run();