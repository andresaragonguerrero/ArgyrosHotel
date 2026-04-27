namespace argyros_hotel_api.Application.DTOs
{
    public class RoomTypeResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public int Capacity { get; set; }

        public int TotalRooms { get; set; }
    }
}