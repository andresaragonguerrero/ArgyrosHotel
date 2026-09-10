using argyros_hotel_api.Application.DTOs;
using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Services;
using argyros_hotel_api.Domain.Users;
using argyros_hotel_api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Puerto por defecto de Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ISavingService, SavingService>();

builder.Services.AddScoped<AvailabilityService>();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseStaticFiles();

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
        request.NumberOfGuests
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
        request.NumberOfGuests
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

// Endpoint para verificar la disponibilidad de un RoomType en un rango de fechas
app.MapGet("/availability", async (
    Guid roomTypeId,
    DateTime startDate,
    DateTime endDate,
    IRoomTypeRepository roomTypeRepo,
    IBookingRepository bookingRepo,
    [FromServices] AvailabilityService availabilityService) =>
{
    var roomType = await roomTypeRepo.GetByIdAsync(roomTypeId);
    if (roomType is null)
        return Results.BadRequest("RoomType no encontrado");

    var bookings = await bookingRepo
        .GetBookingsByRoomTypeAndDateRangeAsync(
            roomTypeId,
            startDate,
            endDate);

    var result = availabilityService.CheckAvailability(
        roomType,
        bookings,
        startDate,
        endDate
    );

    if (result.IsAvailable)
    {
        return Results.Ok(result);
    }

    var allBookings = await bookingRepo.GetAllAsync();

    var nextDate = availabilityService.FindNextAvailableDate(
        roomType,
        allBookings.Where(b => b.RoomTypeId == roomTypeId),
        endDate
    );

    var finalResult = new AvailabilityResult(
        false,
        0,
        nextDate
    );

    return Results.Ok(finalResult);
});

// Endpoint para calcular el precio base de una reserva (sin servicios adicionales)
app.MapGet("/pricing", async (
    Guid roomTypeId,
    DateTime startDate,
    DateTime endDate,
    [FromServices] IRoomTypeRepository roomTypeRepo,
    [FromServices] IPricingService pricingService) =>
{
    var roomType = await roomTypeRepo.GetByIdAsync(roomTypeId);
    if (roomType == null)
        return Results.NotFound("RoomType no encontrado");

    decimal basePrice;
    try
    {
        basePrice = pricingService.CalculateBasePrice(roomType, startDate, endDate);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return Results.Ok(new { BasePrice = basePrice });
});

// Endpoint para calcular el precio final con descuento
app.MapGet("/discount", async (
    Guid userId,
    decimal basePrice,
    IUserRepository userRepo,
    [FromServices] IDiscountService discountService) =>
{
    var user = await userRepo.GetByIdAsync(userId);
    if (user is null)
        return Results.NotFound("Usuario no encontrado");

    decimal finalPrice;
    try
    {
        finalPrice = discountService.ApplyDiscount(user, basePrice);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    return Results.Ok(new
    {
        UserId = userId,
        BasePrice = basePrice,
        FinalPrice = finalPrice,
        user.IsPremium
    });
});

// Endpoint para calcular el precio final de una reserva para un usuario
app.MapGet("/calculatePrice", async (
    Guid userId,
    Guid roomTypeId,
    DateTime startDate,
    DateTime endDate,
    IUserRepository userRepo,
    IRoomTypeRepository roomTypeRepo,
    [FromServices] IPricingService pricingService,
    [FromServices] IDiscountService discountService) =>
{
    // Recuperar usuario
    var user = await userRepo.GetByIdAsync(userId);
    if (user is null)
        return Results.NotFound("Usuario no encontrado");

    // Recuperar RoomType
    var roomType = await roomTypeRepo.GetByIdAsync(roomTypeId);
    if (roomType is null)
        return Results.NotFound("RoomType no encontrado");

    // Calcular precio base
    decimal basePrice;
    try
    {
        basePrice = pricingService.CalculateBasePrice(roomType, startDate, endDate);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    // Aplicar descuento
    decimal finalPrice;
    try
    {
        finalPrice = discountService.ApplyDiscount(user, basePrice);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }

    // Devolver resultado
    return Results.Ok(new
    {
        UserId = userId,
        RoomTypeId = roomTypeId,
        StartDate = startDate,
        EndDate = endDate,
        BasePrice = basePrice,
        FinalPrice = finalPrice,
        user.IsPremium
    });
});

app.MapGet("/savings", async (
    Guid userId,
    IUserRepository userRepo,
    IBookingRepository bookingRepo,
    IRoomTypeRepository roomTypeRepo,
    [FromServices] ISavingService savingService) =>
{
    // Obtener usuario
    var user = await userRepo.GetByIdAsync(userId);
    if (user is null)
        return Results.NotFound("Usuario no encontrado");

    // Obtener reservas del usuario
    var bookings = await bookingRepo.GetAllAsync();
    var userBookings = bookings.Where(b => b.UserId == userId);

    // Obtener todos los tipos de habitación
    var roomTypes = await roomTypeRepo.GetAllAsync();

    // Calcular ahorro
    var totalSavings = savingService.CalculateTotalSavings(
        user,
        userBookings,
        roomTypes
    );

    return Results.Ok(new
    {
        UserId = userId,
        TotalSavings = totalSavings,
        user.IsPremium
    });
});

app.Run();