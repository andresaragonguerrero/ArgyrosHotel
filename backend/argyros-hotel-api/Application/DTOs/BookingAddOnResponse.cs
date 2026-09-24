namespace argyros_hotel_api.Application.DTOs
{
    public class BookingAddOnResponse
    {
        public Guid Id { get; set; }

        public Guid BookingId { get; set; }

        public Guid ServiceId { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}