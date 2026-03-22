using argyros_hotel_api.Aplication.Interfaces;
using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias: repositorios
// Inyección de dependencias
builder.Services.AddSingleton<IUserRepository>(_ =>
    new InMemoryUserRepository(Path.Combine("Infrastructure", "Data", "users.json")));

builder.Services.AddSingleton<IBookingRepository>(_ =>
    new InMemoryBookingRepository(Path.Combine("Infrastructure", "Data", "bookings.json")));

builder.Services.AddSingleton<IRoomTypeRepository>(_ =>
    new InMemoryRoomTypeRepository(Path.Combine("Infrastructure", "Data", "roomTypes.json")));

builder.Services.AddSingleton<IRoomRepository>(_ =>
    new InMemoryRoomRepository(Path.Combine("Infrastructure", "Data", "rooms.json")));

builder.Services.AddSingleton<IServiceRepository>(_ =>
    new InMemoryServiceRepository(Path.Combine("Infrastructure", "Data", "services.json")));

builder.Services.AddSingleton<IBookingServiceRepository>(_ =>
    new InMemoryBookingServiceRepository(Path.Combine("Infrastructure", "Data", "bookingServices.json")));

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

// Users
app.MapGet("/users", async (IUserRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/users/{id}", async (Guid id, IUserRepository repo) =>
{
    var user = await repo.GetByIdAsync(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});

// Bookings

app.MapGet("/bookings", async (IBookingRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/bookings/{id}", async (Guid id, IBookingRepository repo) =>
{
    var booking = await repo.GetByIdAsync(id);
    return booking is null ? Results.NotFound() : Results.Ok(booking);
});

// RoomTypes

app.MapGet("/roomTypes", async (IRoomTypeRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/roomTypes/{id}", async (Guid id, IRoomTypeRepository repo) =>
{
    var roomType = await repo.GetByIdAsync(id);
    return roomType is null ? Results.NotFound() : Results.Ok(roomType);
});

// Rooms

app.MapGet("/rooms", async (IRoomRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/rooms/{id}", async (Guid id, IRoomRepository repo) =>
{
    var room = await repo.GetByIdAsync(id);
    return room is null ? Results.NotFound() : Results.Ok(room);
});

app.MapGet("/rooms/byRoomType/{roomTypeId}", async (Guid roomTypeId, IRoomRepository repo) =>
{
    return Results.Ok(await repo.GetByRoomTypeIdAsync(roomTypeId));
});

// Services

app.MapGet("/services", async (IServiceRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/services/{id}", async (Guid id, IServiceRepository repo) =>
{
    var service = await repo.GetByIdAsync(id);
    return service is null ? Results.NotFound() : Results.Ok(service);
});

// BookingServices

app.MapGet("/bookingServices", async (IBookingServiceRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

app.MapGet("/bookingServices/{id}", async (Guid id, IBookingServiceRepository repo) =>
{
    var bs = await repo.GetByIdAsync(id);
    return bs is null ? Results.NotFound() : Results.Ok(bs);
});

app.MapGet("/bookingServices/byBooking/{bookingId}", async (Guid bookingId, IBookingServiceRepository repo) =>
{
    return Results.Ok(await repo.GetByBookingIdAsync(bookingId));
});

app.Run();