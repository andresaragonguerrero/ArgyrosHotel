namespace argyros_hotel_api.Application.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsPremium { get; set; }
    }
}