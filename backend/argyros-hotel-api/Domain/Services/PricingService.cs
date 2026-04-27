using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Domain.Services
{
    public interface IPricingService
    {
        decimal CalculateBasePrice(RoomType roomType, DateTime startDate, DateTime endDate);
    }

    // Servicio encargado de la lógica relacionada con el cálculo del precio de las reservas
    // NO se incluirá la lógica del cálculo de impuestos, descuentos o promociones:
    // esta funcionalidad se delegará a otros servicios especializados (DiscountService, TaxService)
    public class PricingService : IPricingService
    {
        public decimal CalculateBasePrice(
                    RoomType roomType,
                    DateTime startDate,
                    DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Invalid date range");

            // Calcular tiempo de la estancia en noches
            var nights = (endDate - startDate).Days;

            if (nights <= 0)
                throw new ArgumentException("Stay must be at least one night");

            // Calcular precio base
            var totalPrice = nights * roomType.BasePrice;

            return totalPrice;
        }// CalculateTotalPrice method.end
    } // PricingService class.end
}
