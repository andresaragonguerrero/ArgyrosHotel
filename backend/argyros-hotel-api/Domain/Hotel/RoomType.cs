using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Hotel
{
    // Representa un tipo de habitación: individual, doble, suite, etc
    // Cada habitación tendrá una capacidad, un precio base y un número determinado de habitaciones disponibles
    public class RoomType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public decimal BasePrice { get; private set; }
        public int TotalRooms { get; private set; }

        [JsonConstructor]
        public RoomType(Guid id, string name, int capacity, decimal basePrice, int totalRooms)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la habitación no puede estar vacío.", nameof(name));
            if (capacity <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que cero.", nameof(capacity));
            if (basePrice < 0)
                throw new ArgumentException("El precio base no puede ser negativo.", nameof(basePrice));
            if (totalRooms < 0)
                throw new ArgumentException("El número total de habitaciones no puede ser negativo.", nameof(totalRooms));

            Id = id;
            Name = name;
            Capacity = capacity;
            BasePrice = basePrice;
            TotalRooms = totalRooms;
        }
    }
}