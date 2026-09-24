using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryBookingAddOnRepository : IBookingAddOnRepository
    {
        private readonly List<BookingAddOn> _BookingAddOns;

        public InMemoryBookingAddOnRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _BookingAddOns = new List<BookingAddOn>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _BookingAddOns = JsonSerializer.Deserialize<List<BookingAddOn>>(json, options)
                                   ?? new List<BookingAddOn>();
            }
        }

        public Task<BookingAddOn?> GetByIdAsync(Guid id) =>
            Task.FromResult(_BookingAddOns.FirstOrDefault(bs => bs.Id == id));

        public Task<IEnumerable<BookingAddOn>> GetAllAsync() =>
            Task.FromResult(_BookingAddOns.AsEnumerable());

        public Task<IEnumerable<BookingAddOn>> GetByBookingIdAsync(Guid bookingId)
        {
            var services = _BookingAddOns.Where(bs => bs.BookingId == bookingId);
            return Task.FromResult(services.AsEnumerable());
        }

        public Task AddAsync(BookingAddOn BookingAddOn)
        {
            _BookingAddOns.Add(BookingAddOn);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var existing = _BookingAddOns.FirstOrDefault(bs => bs.Id == id);
            if (existing != null)
                _BookingAddOns.Remove(existing);

            return Task.CompletedTask;
        }
    }
}
