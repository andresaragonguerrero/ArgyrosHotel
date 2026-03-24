using argyros_hotel_api.Aplication.DTOs;
using argyros_hotel_api.Aplication.Interfaces;
using argyros_hotel_api.Application.DTOs;
using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Users;
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

// Falta por implementar el hash de contraseña (también validaciones, es un MVP)
app.MapPost("/users", async (CreateUserRequest request, IUserRepository repo) =>
{
    var passwordHash = request.Password;

    var user = new User(
        Guid.NewGuid(),
        request.Name,
        request.Surname,
        request.Email,
        passwordHash,
        request.IsPremium
    );

    await repo.AddAsync(user);

    var response = new UserResponse
    {
        Id = user.Id,
        Name = user.Name,
        Surname = user.Surname,
        Email = user.Email,
        IsPremium = user.IsPremium
    };

    return Results.Created($"/users/{user.Id}", response);
});

app.MapPut("/users/{id}", async (Guid id, UpdateUserRequest request, IUserRepository repo) =>
{
    var existing = await repo.GetByIdAsync(id);
    if (existing is null) return Results.NotFound();

    var updated = new User(
        id,
        request.Name,
        request.Surname,
        request.Email,
        request.Password,
        request.IsPremium
    );

    await repo.UpdateAsync(updated);

    var response = new UserResponse
    {
        Id = updated.Id,
        Name = updated.Name,
        Surname = updated.Surname,
        Email = updated.Email,
        IsPremium = updated.IsPremium
    };

    return Results.Ok(response);
});

app.MapDelete("/users/{id}", async (Guid id, IUserRepository repo) =>
{
    var existing = await repo.GetByIdAsync(id);
    if (existing is null) return Results.NotFound();

    await repo.DeleteAsync(id);
    return Results.NoContent();
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

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    IBookingRepository bookingRepo,
    IRoomTypeRepository roomTypeRepo,
    IUserRepository userRepo) =>
{
    var roomType = await roomTypeRepo.GetByIdAsync(request.RoomTypeId);
    if (roomType is null)
        return Results.BadRequest("RoomType no encontrado");

    var user = await userRepo.GetByIdAsync(request.UserId);
    if (user is null)
        return Results.BadRequest("Usuario no encontrado");

    var existingBookings = await bookingRepo
        .GetBookingsByRoomTypeAndDateRangeAsync(
            request.RoomTypeId,
            request.StartDate,
            request.EndDate);

    if (existingBookings.Count() >= roomType.TotalRooms)
        return Results.BadRequest("No hay disponibilidad");

    var booking = new Booking(
        Guid.NewGuid(),
        request.UserId,
        request.RoomTypeId,
        request.StartDate,
        request.EndDate,
        request.NumberOfGuests,
        0m // PricingService
    );

    await bookingRepo.AddAsync(booking);

    return Results.Created($"/bookings/{booking.Id}", booking);
});

app.MapPut("/bookings/{id}", async (
    Guid id,
    UpdateBookingRequest request,
    IBookingRepository bookingRepo,
    IRoomTypeRepository roomTypeRepo) =>
{
    var existing = await bookingRepo.GetByIdAsync(id);
    if (existing is null)
        return Results.NotFound();

    var roomType = await roomTypeRepo.GetByIdAsync(request.RoomTypeId);
    if (roomType is null)
        return Results.BadRequest("RoomType no encontrado");

    var updated = new Booking(
        id,
        request.UserId,
        request.RoomTypeId,
        request.StartDate,
        request.EndDate,
        request.NumberOfGuests,
        0m
    );

    await bookingRepo.UpdateAsync(updated);

    return Results.Ok(updated);
});

app.MapDelete("/bookings/{id}", async (Guid id, IBookingRepository repo) =>
{
    var existing = await repo.GetByIdAsync(id);
    if (existing is null)
        return Results.NotFound();

    await repo.DeleteAsync(id);
    return Results.NoContent();
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

app.MapPost("/bookingServices", async (
    CreateBookingServiceRequest request,
    IBookingServiceRepository repo,
    IBookingRepository bookingRepo,
    IServiceRepository serviceRepo) =>
{
    var booking = await bookingRepo.GetByIdAsync(request.BookingId);
    if (booking is null)
        return Results.BadRequest("Booking no encontrado");

    var service = await serviceRepo.GetByIdAsync(request.ServiceId);
    if (service is null)
        return Results.BadRequest("Service no encontrado");

    var bookingService = new BookingService(
        Guid.NewGuid(),
        request.BookingId,
        request.ServiceId,
        service.Price
    );

    await repo.AddAsync(bookingService);

    return Results.Created($"/bookingServices/{bookingService.Id}", bookingService);
});

app.MapDelete("/bookingServices/{id}", async (Guid id, IBookingServiceRepository repo) =>
{
    var existing = await repo.GetByIdAsync(id);
    if (existing is null)
        return Results.NotFound();

    await repo.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();