using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryBookingAddOnRepository : IBookingAddOnRepository
    {
        private readonly List<BookingAddOn> _bookingAddOns;
        private readonly string _jsonFilePath;

        public InMemoryBookingAddOnRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;

            if (!File.Exists(jsonFilePath))
                _bookingAddOns = new List<BookingAddOn>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _bookingAddOns = JsonSerializer.Deserialize<List<BookingAddOn>>(json, options)
                                   ?? new List<BookingAddOn>();
            }
        }

        public Task<BookingAddOn?> GetByIdAsync(Guid id) =>
            Task.FromResult(_bookingAddOns.FirstOrDefault(bs => bs.Id == id));

        public Task<IEnumerable<BookingAddOn>> GetAllAsync() =>
            Task.FromResult(_bookingAddOns.AsEnumerable());

        public Task<IEnumerable<BookingAddOn>> GetByBookingIdAsync(Guid bookingId)
        {
            var addOns = _bookingAddOns.Where(bs => bs.BookingId == bookingId);
            return Task.FromResult(addOns.AsEnumerable());
        }

        public async Task AddAsync(BookingAddOn bookingAddOn)
        {
            _bookingAddOns.Add(bookingAddOn);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = _bookingAddOns.FirstOrDefault(bs => bs.Id == id);
            if (existing != null)
            {
                _bookingAddOns.Remove(existing);
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_bookingAddOns, options);
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
    }
}