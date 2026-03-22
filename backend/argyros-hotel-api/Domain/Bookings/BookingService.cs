using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Bookings
{
    // Clase que representa a un servicio adicional asociado a una reserva de habitación de hotel
    public class BookingService
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public Guid ServiceId { get; private set; }
        public decimal TotalPrice { get; private set; }

        [JsonConstructor]
        public BookingService(Guid id, Guid bookingId, Guid serviceId, decimal totalPrice)
        {
            if (bookingId == Guid.Empty)
                throw new ArgumentException("BookingId inválido", nameof(bookingId));

            if (serviceId == Guid.Empty)
                throw new ArgumentException("ServiceId inválido", nameof(serviceId));

            if (totalPrice < 0)
                throw new ArgumentException("El precio total no puede ser negativo", nameof(totalPrice));

            Id = id;
            BookingId = bookingId;
            ServiceId = serviceId;
            TotalPrice = totalPrice;
        }
    }
}