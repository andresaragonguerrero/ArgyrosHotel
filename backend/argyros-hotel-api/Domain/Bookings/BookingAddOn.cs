using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Bookings
{
    // Clase que representa a un servicio adicional asociado a una reserva de habitación de hotel
    public class BookingAddOn
    {
        public Guid Id { get; private set; }

        public Guid BookingId { get; private set; }

        public Guid ServiceId { get; private set; }

        public decimal TotalPrice { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        [JsonConstructor]
        public BookingAddOn(Guid id, Guid bookingId, Guid serviceId, int quantity, decimal unitPrice, decimal totalPrice)
        {
            if (bookingId == Guid.Empty) throw new ArgumentException("BookingId inválido", nameof(bookingId));
            if (serviceId == Guid.Empty) throw new ArgumentException("ServiceId inválido", nameof(serviceId));
            if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor que cero", nameof(quantity));
            if (unitPrice < 0) throw new ArgumentException("El precio unitario no puede ser negativo", nameof(unitPrice));
            if (totalPrice < 0) throw new ArgumentException("El precio total no puede ser negativo", nameof(totalPrice));

            Id = id;
            BookingId = bookingId;
            ServiceId = serviceId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
        }
    }
}