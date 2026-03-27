namespace argyros_hotel_api.Application.DTOs
{
    public class BookingServiceResponse
    {
        public Guid Id { get; set; }

        public Guid BookingId { get; set; }

        public Guid ServiceId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}