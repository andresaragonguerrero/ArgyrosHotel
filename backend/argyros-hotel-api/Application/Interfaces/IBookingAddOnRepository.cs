using argyros_hotel_api.Domain.Bookings;

namespace argyros_hotel_api.Application.Interfaces
{
    public interface IBookingAddOnRepository
    {
        Task<BookingAddOn?> GetByIdAsync(Guid id);
        Task<IEnumerable<BookingAddOn>> GetAllAsync();
        Task<IEnumerable<BookingAddOn>> GetByBookingIdAsync(Guid bookingId);
        Task AddAsync(BookingAddOn BookingAddOn);
        Task DeleteAsync(Guid id);
    }
}
