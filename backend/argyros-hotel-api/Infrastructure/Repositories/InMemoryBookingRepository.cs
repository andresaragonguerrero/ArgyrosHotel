using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings;
        private readonly string _jsonFilePath;

        public InMemoryBookingRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;

            if (!File.Exists(jsonFilePath))
                _bookings = new List<Booking>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _bookings = JsonSerializer.Deserialize<List<Booking>>(json, options)
                            ?? new List<Booking>();
            }
        }

        public async Task AddAsync(Booking booking)
        {
            _bookings.Add(booking);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                _bookings.Remove(booking);
                await SaveAsync();
            }
        }

        public Task<IEnumerable<Booking>> GetAllAsync() => Task.FromResult(_bookings.AsEnumerable());

        public Task<Booking?> GetByIdAsync(Guid id) =>
            Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));

        public Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId) =>
            Task.FromResult(_bookings.Where(b => b.UserId == userId).AsEnumerable());

        public Task<IEnumerable<Booking>> GetBookingsByRoomTypeAndDateRangeAsync(Guid roomTypeId, DateTime startDate, DateTime endDate)
        {
            var overlapping = _bookings.Where(b =>
                b.RoomTypeId == roomTypeId &&
                b.StartDate < endDate &&
                b.EndDate > startDate);
            return Task.FromResult(overlapping.AsEnumerable());
        }

        public async Task UpdateAsync(Booking booking)
        {
            var index = _bookings.FindIndex(b => b.Id == booking.Id);
            if (index >= 0)
            {
                _bookings[index] = booking;
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_bookings, options);
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
    }
}