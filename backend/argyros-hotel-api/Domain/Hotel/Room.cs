using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Hotel
{
    // Clase que representa a una habitación de hotel
    public class Room
    {
        public Guid Id { get; private set; }
        public string RoomNumber { get; private set; }
        public Guid RoomTypeId { get; private set; }

        [JsonConstructor]
        public Room(Guid id, string roomNumber, Guid roomTypeId)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                throw new ArgumentException("El número de habitación no puede estar vacío", nameof(roomNumber));

            if (roomTypeId == Guid.Empty)
                throw new ArgumentException("RoomTypeId inválido", nameof(roomTypeId));

            Id = id;
            RoomNumber = roomNumber;
            RoomTypeId = roomTypeId;
        }
    }
}