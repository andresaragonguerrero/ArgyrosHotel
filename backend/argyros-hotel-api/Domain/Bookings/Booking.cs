namespace ArgrosHotel.Domain.Bookings
{
    // Clase que representa una reserva de habitación de hotel
    // En lugar de incluir aquí el cálculo total,
    // asumiré que se calculará en dos servicios separados:
    // - BookingService/PricingService: donde se calculará el precio total de la reserva basándose en el tipo de habitación, fechas, número de huéspedes, etc
    // - DiscountService: donde se aplicarán los descuentos correspondientes basándose en las promociones, el historial del cliente, si es premium, etc
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid RoomTypeId { get; private set; }
        public Guid? RoomId { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int NumberOfGuests { get; private set; }

        public Booking(Guid id, Guid userId, Guid roomTypeId, DateTime startDate, DateTime endDate, int numberOfGuests)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId inválido", nameof(userId));

            if (roomTypeId == Guid.Empty)
                throw new ArgumentException("RoomTypeId inválido", nameof(roomTypeId));

            if (startDate >= endDate)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la de fin");

            if (numberOfGuests <= 0)
                throw new ArgumentException("Debe haber al menos un huésped", nameof(numberOfGuests));

            Id = id;
            UserId = userId;
            RoomTypeId = roomTypeId;
            StartDate = startDate;
            EndDate = endDate;
            NumberOfGuests = numberOfGuests;
        }

        public void AssignRoom(Guid roomId)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("RoomId inválido", nameof(roomId));

            RoomId = roomId;
        }
    }
}