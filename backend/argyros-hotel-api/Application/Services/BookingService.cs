using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Services;

namespace argyros_hotel_api.Application.Services
{
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
            int adults,
            int children,
            int seniors)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("User not found");

            var roomType = await _roomTypeRepository.GetByIdAsync(roomTypeId)
                          ?? throw new Exception("Room type not found");

            var allBookings = await _bookingRepository.GetAllAsync();
            var bookingsForType = allBookings
                .Where(b => b.RoomTypeId == roomTypeId)
                .ToList();

            var availability = _availabilityService.CheckAvailability(
                roomType,
                bookingsForType,
                startDate,
                endDate
            );

            if (!availability.IsAvailable)
                throw new Exception("No rooms available for selected dates");

            var basePrice = _pricingService.CalculateBasePrice(
                roomType,
                startDate,
                endDate,
                adults,
                children,
                seniors
            );

            var finalPrice = _discountService.ApplyDiscount(user, basePrice);

            var booking = new Booking(
                Guid.NewGuid(),
                user.Id,
                roomType.Id,
                startDate,
                endDate,
                adults,
                children,
                seniors,
                basePrice,
                finalPrice
            );

            await _bookingRepository.AddAsync(booking);

            return booking;
        }
    }
}