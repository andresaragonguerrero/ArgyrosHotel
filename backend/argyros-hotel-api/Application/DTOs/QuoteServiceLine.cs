namespace argyros_hotel_api.Application.DTOs
{
    public class QuoteServiceLine
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}