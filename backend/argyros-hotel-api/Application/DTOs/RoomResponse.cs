namespace argyros_hotel_api.Application.DTOs
{
    public class RoomResponse
    {
        public Guid Id { get; set; }

        public Guid RoomTypeId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}