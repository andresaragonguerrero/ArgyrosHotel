namespace argyros_hotel_api.Application.DTOs
{
    public class CreateBookingServiceRequest
    {
        public Guid BookingId { get; set; }

        public Guid ServiceId { get; set; }
    }
}