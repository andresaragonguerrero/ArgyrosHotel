namespace argyros_hotel_api.Aplication.DTOs
{
    public class UpdateBookingRequest
    {
        public Guid UserId { get; set; }

        public Guid RoomTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfGuests { get; set; }
    }
}
