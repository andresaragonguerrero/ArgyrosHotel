using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Domain.Services
{
    public interface IPricingService
    {
        decimal CalculateBasePrice(
            RoomType roomType,
            DateTime startDate,
            DateTime endDate,
            int adults,
            int children,
            int seniors);
    }

    public class PricingService : IPricingService
    {
        private readonly IGuestSurchargeService _surchargeService;

        public PricingService(IGuestSurchargeService surchargeService)
        {
            _surchargeService = surchargeService;
        }

        public decimal CalculateBasePrice(
            RoomType roomType,
            DateTime startDate,
            DateTime endDate,
            int adults,
            int children,
            int seniors)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Invalid date range");

            var nights = (endDate - startDate).Days;

            if (nights <= 0)
                throw new ArgumentException("Stay must be at least one night");

            var extraPerNight = _surchargeService.CalculateExtraPerNight(adults, children, seniors);

            return nights * (roomType.BasePrice + extraPerNight);
        }
    }
}