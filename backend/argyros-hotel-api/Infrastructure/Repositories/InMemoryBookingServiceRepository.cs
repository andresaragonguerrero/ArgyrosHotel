using argyros_hotel_api.Aplication.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryBookingServiceRepository : IBookingServiceRepository
    {
        private readonly List<BookingService> _bookingServices;

        public InMemoryBookingServiceRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _bookingServices = new List<BookingService>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _bookingServices = JsonSerializer.Deserialize<List<BookingService>>(json, options)
                                   ?? new List<BookingService>();
            }
        }

        public Task<BookingService?> GetByIdAsync(Guid id) =>
            Task.FromResult(_bookingServices.FirstOrDefault(bs => bs.Id == id));

        public Task<IEnumerable<BookingService>> GetAllAsync() =>
            Task.FromResult(_bookingServices.AsEnumerable());

        public Task<IEnumerable<BookingService>> GetByBookingIdAsync(Guid bookingId)
        {
            var services = _bookingServices.Where(bs => bs.BookingId == bookingId);
            return Task.FromResult(services.AsEnumerable());
        }
    }
}
