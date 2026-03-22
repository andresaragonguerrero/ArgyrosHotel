using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Aplication.Interfaces
{
    // En esta interfaz necesitaré obtener las habitaciones disponibles,
    // obtenerlas por tipos y por asignación en las reservas
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id);
        Task<IEnumerable<Room>> GetAllAsync();
        Task<IEnumerable<Room>> GetByRoomTypeIdAsync(Guid roomTypeId);
    }
}
