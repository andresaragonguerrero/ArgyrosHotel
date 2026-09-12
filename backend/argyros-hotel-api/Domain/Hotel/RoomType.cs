using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Hotel
{
    public class RoomType
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public int Capacity { get; private set; }

        public decimal BasePrice { get; private set; }

        public int TotalRooms { get; private set; }

        public string ImageUrl { get; private set; }

        public string Description { get; private set; }

        public int SquareMeters { get; private set; }

        [JsonConstructor]
        public RoomType(Guid id, string name, int capacity, decimal basePrice, int totalRooms, string imageUrl, string description, int squareMeters)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la habitación no puede estar vacío.", nameof(name));
            if (capacity <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que cero.", nameof(capacity));
            if (basePrice < 0)
                throw new ArgumentException("El precio base no puede ser negativo.", nameof(basePrice));
            if (totalRooms < 0)
                throw new ArgumentException("El número total de habitaciones no puede ser negativo.", nameof(totalRooms));
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("La URL de la imagen no puede estar vacía.", nameof(imageUrl));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(description));
            if (squareMeters <= 0)
                throw new ArgumentException("Los metros cuadrados deben ser mayores que cero.", nameof(squareMeters));

            Id = id;
            Name = name;
            Capacity = capacity;
            BasePrice = basePrice;
            TotalRooms = totalRooms;
            ImageUrl = imageUrl;
            Description = description;
            SquareMeters = squareMeters;
        }
    }
}