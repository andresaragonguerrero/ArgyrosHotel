namespace argyros_hotel_api.Application.DTOs
{
    public class QuoteResponse
    {
        public decimal BasePrice { get; set; }
        public decimal RoomFinalPrice { get; set; }
        public bool IsPremium { get; set; }
        public List<QuoteServiceLine> Services { get; set; } = new();
        public decimal ServicesTotal { get; set; }
        public decimal GrandTotal { get; set; }
    }
}