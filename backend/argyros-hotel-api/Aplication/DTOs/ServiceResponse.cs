namespace argyros_hotel_api.Application.DTOs
{
    public class ServiceResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}