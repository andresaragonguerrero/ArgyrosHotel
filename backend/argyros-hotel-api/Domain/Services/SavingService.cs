using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Hotel;
using argyros_hotel_api.Domain.Users;

namespace argyros_hotel_api.Domain.Services
{
    public interface ISavingService
    {
        decimal CalculateTotalSavings(
            User user,
            IEnumerable<Booking> bookings,
            IEnumerable<RoomType> roomTypes
        );
    }

    // Servicio encargado de manejar la lógica de negocio relacionada con 
    // calcular el total de los ahorroros de un cliente por el mero hecho de ser premium
    public class SavingService : ISavingService
    {
        private readonly IPricingService _pricingService;
        private readonly IDiscountService _discountService;

        public SavingService(
            IPricingService pricingService,
            IDiscountService discountService)
        {
            _pricingService = pricingService;
            _discountService = discountService;
        }

        public decimal CalculateTotalSavings(
            User user,
            IEnumerable<Booking> bookings,
            IEnumerable<RoomType> roomTypes)
        {
            if (!user.IsPremium)
                return 0m;

            decimal totalSavings = 0m;

            foreach (var booking in bookings)
            {
                var roomType = roomTypes
                    .FirstOrDefault(rt => rt.Id == booking.RoomTypeId);

                if (roomType is null)
                    continue;

                var basePrice = _pricingService.CalculateBasePrice(
                    roomType,
                    booking.StartDate,
                    booking.EndDate);

                var finalPrice = _discountService.ApplyDiscount(user, basePrice);

                var savings = basePrice - finalPrice;
                totalSavings += savings;
            }

            return totalSavings;
        }
    }
}
