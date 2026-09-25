namespace argyros_hotel_api.Application.DTOs
{
    public class BookingResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomTypeId { get; set; }

        public Guid? RoomId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfGuests { get; set; }

        public decimal BasePrice { get; set; }
        
        public decimal FinalPrice { get; set; }

        public List<BookingAddOnResponse> AddOns { get; set; } = new();
    }
}