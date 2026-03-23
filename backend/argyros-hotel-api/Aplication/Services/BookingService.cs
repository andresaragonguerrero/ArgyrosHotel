namespace argyros_hotel_api.Aplication.Services
{
    // Archivo encargado de orquestar los servicios relacionados con las reservas 
    public class BookingService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        private readonly AvailabilityService _availabilityService;
        private readonly PricingService _pricingService;
        private readonly DiscountService _discountService;

        public BookingService(
            IUserRepository userRepository,
            IBookingRepository bookingRepository,
            IRoomTypeRepository roomTypeRepository,
            AvailabilityService availabilityService,
            PricingService pricingService,
            DiscountService discountService)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _roomTypeRepository = roomTypeRepository;
            _availabilityService = availabilityService;
            _pricingService = pricingService;
            _discountService = discountService;
        }

        public async Task<Booking> CreateBookingAsync(
            Guid userId,
            Guid roomTypeId,
            DateTime startDate,
            DateTime endDate,
            int numberOfGuests)
        {
            // Obtener usuario
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            // Obtener tipo de habitación
            var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId)
                          ?? throw new Exception("Room type not found");

            // Obtener reservas existentes de ese tipo
            var allBookings = await _bookingRepository.GetAllAsync();
            var bookingsForType = allBookings
                .Where(b => b.RoomTypeId == roomTypeId)
                .ToList();

            // Comprobar disponibilidad
            var availability = _availabilityService.CheckAvailability(
                roomType,
                bookingsForType,
                startDate,
                endDate
            );

            if (!availability.IsAvailable)
                throw new Exception("No rooms available for selected dates");

            // Calcular precio base
            var basePrice = _pricingService.CalculateBasePrice(
                roomType,
                startDate,
                endDate
            );

            // Aplicar descuentos
            var finalPrice = _discountService.ApplyDiscount(user, basePrice);

            // Crear reserva
            var booking = new Booking(
                Guid.NewGuid(),
                user.Id,
                roomType.Id,
                startDate,
                endDate,
                numberOfGuests,
                finalPrice
            );

            // Guardar
            await _bookingRepository.AddAsync(booking);

            return booking;
        }
    }
}
