using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Hotel
{
    // Representa un servicio adicional que puede ser contratado por los huéspedes: desayuno, actividad, evento, etc
    public class Service
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        [JsonConstructor]
        public Service(Guid id, string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

            Id = id;
            Name = name;
            Price = price;
        }
    }
}