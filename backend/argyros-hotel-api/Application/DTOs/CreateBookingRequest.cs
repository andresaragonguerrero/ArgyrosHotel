namespace argyros_hotel_api.Application.DTOs
{
    public class CreateBookingRequest
    {
        public Guid UserId { get; set; }
        public Guid RoomTypeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int NumberOfGuests { get; set; }
    }
}