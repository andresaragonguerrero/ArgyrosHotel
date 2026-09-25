using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Hotel
{
    public class Service
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        [JsonConstructor]
        public Service(Guid id, string name, string description, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía.", nameof(description));
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}