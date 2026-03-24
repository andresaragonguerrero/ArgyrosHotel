using argyros_hotel_api.Domain.Users;

namespace argyros_hotel_api.Domain.Services
{
    public interface IDiscountService
    {
        decimal ApplyDiscount(User user, decimal basePrice);
    }

    // Servicio encargado de la lógica relacionada con el cálculo de los descuentos aplicables a las reservas 
    public class DiscountService : IDiscountService
    {
        // Tasa de descuento para los usuarios premium (10% de descuento actualmente)
        private const decimal PremiumDiscountRate = 0.10m;

        public decimal ApplyDiscount(User user, decimal basePrice)
        {
            if (basePrice < 0)
                throw new ArgumentException("Base price cannot be negative");

            decimal finalPrice = basePrice;

            // Descuento por usuario premium
            if (user.IsPremium)
            {
                var discountAmount = basePrice * PremiumDiscountRate;
                finalPrice -= discountAmount;
            }

            return finalPrice;
        } // ApplyDiscount method.end
    } // DiscountService class.end
}
