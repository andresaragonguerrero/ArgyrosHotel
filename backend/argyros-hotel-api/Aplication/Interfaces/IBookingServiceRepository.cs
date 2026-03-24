using argyros_hotel_api.Domain.Bookings;

namespace argyros_hotel_api.Aplication.Interfaces
{
    public interface IBookingServiceRepository
    {
        Task<BookingService?> GetByIdAsync(Guid id);
        Task<IEnumerable<BookingService>> GetAllAsync();
        Task<IEnumerable<BookingService>> GetByBookingIdAsync(Guid bookingId);
        Task AddAsync(BookingService bookingService);
        Task DeleteAsync(Guid id);
    }
}
