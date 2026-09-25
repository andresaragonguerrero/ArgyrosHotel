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

builder.Services.AddSingleton<IBookingAddOnRepository>(_ =>
    new InMemoryBookingAddOnRepository(Path.Combine("Infrastructure", "Data", "bookingAddOns.json")));

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
    var bookings = await repo.GetAllAsync();
    var response = bookings.Select(b => new BookingResponse
    {
        Id = b.Id,
        UserId = b.UserId,
        RoomTypeId = b.RoomTypeId,
        RoomId = b.RoomId,
        StartDate = b.StartDate,
        EndDate = b.EndDate,
        NumberOfGuests = b.NumberOfGuests,
        BasePrice = b.BasePrice,
        FinalPrice = b.FinalPrice
    });
    return Results.Ok(response);
});

app.MapGet("/bookings/{id}", async (Guid id, IBookingRepository repo, IBookingAddOnRepository addOnRepo) =>
{
    var booking = await repo.GetByIdAsync(id);
    if (booking is null) return Results.NotFound();

    var addOns = await addOnRepo.GetByBookingIdAsync(id);

    var response = new BookingResponse
    {
        Id = booking.Id,
        UserId = booking.UserId,
        RoomTypeId = booking.RoomTypeId,
        RoomId = booking.RoomId,
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        NumberOfGuests = booking.NumberOfGuests,
        BasePrice = booking.BasePrice,
        FinalPrice = booking.FinalPrice,
        AddOns = addOns.Select(a => new BookingAddOnResponse
        {
            Id = a.Id,
            BookingId = a.BookingId,
            ServiceId = a.ServiceId,
            Quantity = a.Quantity,
            UnitPrice = a.UnitPrice,
            TotalPrice = a.TotalPrice
        }).ToList()
    };
    return Results.Ok(response);
});

app.MapPost("/bookings", async (
    CreateBookingRequest request,
    IBookingRepository bookingRepo,
    IRoomTypeRepository roomTypeRepo,
    IUserRepository userRepo,
    [FromServices] IPricingService pricingService,
    [FromServices] IDiscountService discountService) =>
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

    var basePrice = pricingService.CalculateBasePrice(roomType, request.StartDate, request.EndDate);
    var finalPrice = discountService.ApplyDiscount(user, basePrice);

    var booking = new Booking(
        Guid.NewGuid(),
        request.UserId,
        request.RoomTypeId,
        request.StartDate,
        request.EndDate,
        request.NumberOfGuests,
        basePrice,
        finalPrice
    );

    await bookingRepo.AddAsync(booking);

    var response = new BookingResponse
    {
        Id = booking.Id,
        UserId = booking.UserId,
        RoomTypeId = booking.RoomTypeId,
        RoomId = booking.RoomId,
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        NumberOfGuests = booking.NumberOfGuests,
        BasePrice = booking.BasePrice,
        FinalPrice = booking.FinalPrice
    };

    return Results.Created($"/bookings/{booking.Id}", response);
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
        existing.BasePrice,
        existing.FinalPrice
    );

    await bookingRepo.UpdateAsync(updated);

    var response = new BookingResponse
    {
        Id = updated.Id,
        UserId = updated.UserId,
        RoomTypeId = updated.RoomTypeId,
        RoomId = updated.RoomId,
        StartDate = updated.StartDate,
        EndDate = updated.EndDate,
        NumberOfGuests = updated.NumberOfGuests,
        BasePrice = updated.BasePrice,
        FinalPrice = updated.FinalPrice
    };

    return Results.Ok(response);
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
    var services = await repo.GetAllAsync();
    var response = services.Select(s => new ServiceResponse
    {
        Id = s.Id,
        Name = s.Name,
        Description = s.Description,
        Price = s.Price
    });
    return Results.Ok(response);
});

app.MapGet("/services/{id}", async (Guid id, IServiceRepository repo) =>
{
    var service = await repo.GetByIdAsync(id);
    if (service is null) return Results.NotFound();

    var response = new ServiceResponse
    {
        Id = service.Id,
        Name = service.Name,
        Description = service.Description,
        Price = service.Price
    };
    return Results.Ok(response);
});

// BookingServices

app.MapGet("/bookings/{id}/addOns", async (Guid id, IBookingAddOnRepository repo) =>
{
    var addOns = await repo.GetByBookingIdAsync(id);
    var response = addOns.Select(a => new BookingAddOnResponse
    {
        Id = a.Id,
        BookingId = a.BookingId,
        ServiceId = a.ServiceId,
        Quantity = a.Quantity,
        UnitPrice = a.UnitPrice,
        TotalPrice = a.TotalPrice
    });
    return Results.Ok(response);
});

app.MapPost("/bookingAddOns", async (
    CreateBookingAddOnRequest request,
    IBookingAddOnRepository repo,
    IBookingRepository bookingRepo,
    IServiceRepository serviceRepo) =>
{
    var booking = await bookingRepo.GetByIdAsync(request.BookingId);
    if (booking is null)
        return Results.BadRequest("Booking no encontrado");

    var service = await serviceRepo.GetByIdAsync(request.ServiceId);
    if (service is null)
        return Results.BadRequest("Service no encontrado");

    if (request.Quantity <= 0)
        return Results.BadRequest("La cantidad debe ser mayor que cero");

    var existingAddOns = await repo.GetByBookingIdAsync(request.BookingId);
    if (existingAddOns.Any(a => a.ServiceId == request.ServiceId))
        return Results.BadRequest("Este servicio ya está contratado para esta reserva");

    var quantity = request.Quantity;
    var unitPrice = service.Price;
    var totalPrice = unitPrice * quantity;

    var bookingAddOn = new BookingAddOn(
        Guid.NewGuid(),
        request.BookingId,
        request.ServiceId,
        quantity,
        unitPrice,
        totalPrice
    );

    await repo.AddAsync(bookingAddOn);

    var response = new BookingAddOnResponse
    {
        Id = bookingAddOn.Id,
        BookingId = bookingAddOn.BookingId,
        ServiceId = bookingAddOn.ServiceId,
        Quantity = bookingAddOn.Quantity,
        UnitPrice = bookingAddOn.UnitPrice,
        TotalPrice = bookingAddOn.TotalPrice
    };

    return Results.Created($"/bookings/{bookingAddOn.BookingId}/addOns", response);
});

app.MapDelete("/bookingAddOns/{id}", async (Guid id, IBookingAddOnRepository repo) =>
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