using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Bookings
{
    // Clase que representa una reserva de habitaci�n de hotel
    // En lugar de incluir aqu� el c�lculo total,
    // asumir� que se calcular� en dos servicios separados:
    // - BookingService/PricingService: donde se calcular� el precio total de la reserva bas�ndose en el tipo de habitaci�n, fechas, n�mero de hu�spedes, etc
    // - DiscountService: donde se aplicar�n los descuentos correspondientes bas�ndose en las promociones, el historial del cliente, si es premium, etc
    public class Booking
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid RoomTypeId { get; private set; }

        public Guid? RoomId { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public int NumberOfGuests { get; private set; }

        public decimal BasePrice { get; private set; }

        public decimal FinalPrice { get; private set; }

        [JsonConstructor]
        public Booking(Guid id, Guid userId, Guid roomTypeId, DateTime startDate, DateTime endDate, int numberOfGuests, decimal basePrice, decimal finalPrice)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId inválido", nameof(userId));

            if (roomTypeId == Guid.Empty)
                throw new ArgumentException("RoomTypeId inválido", nameof(roomTypeId));

            if (startDate >= endDate)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la de fin");

            if (numberOfGuests <= 0)
                throw new ArgumentException("Debe haber al menos un huésped", nameof(numberOfGuests));

            if (basePrice < 0 || finalPrice < 0) throw new ArgumentException("Los precios no pueden ser negativos");

            Id = id;
            UserId = userId;
            RoomTypeId = roomTypeId;
            StartDate = startDate;
            EndDate = endDate;
            NumberOfGuests = numberOfGuests;
            BasePrice = basePrice;
            FinalPrice = finalPrice;
        }

        public void AssignRoom(Guid roomId)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("RoomId inválido", nameof(roomId));

            RoomId = roomId;
        }
    }
}