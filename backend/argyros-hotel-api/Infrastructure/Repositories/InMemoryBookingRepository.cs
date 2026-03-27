using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Bookings;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    // Implementación básica en memoria del repositorio de reservas
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings;

        public InMemoryBookingRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _bookings = new List<Booking>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                _bookings = JsonSerializer.Deserialize<List<Booking>>(json)
                            ?? new List<Booking>();
            }
        }

        public Task AddAsync(Booking booking)
        {
            _bookings.Add(booking);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
                _bookings.Remove(booking);
            return Task.CompletedTask;
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

        public Task UpdateAsync(Booking booking)
        {
            var index = _bookings.FindIndex(b => b.Id == booking.Id);
            if (index >= 0)
                _bookings[index] = booking;
            return Task.CompletedTask;
        }
    }
}