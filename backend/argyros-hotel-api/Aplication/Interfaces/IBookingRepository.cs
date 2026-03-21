using ArgrosHotel.Domain.Bookings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgrosHotel.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Guid id);

        // Métodos útiles para disponibilidad
        Task<IEnumerable<Booking>> GetBookingsByRoomTypeAndDateRangeAsync(Guid roomTypeId, DateTime startDate, DateTime endDate);
    }
}