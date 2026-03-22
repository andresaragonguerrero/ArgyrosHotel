using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Aplication.Interfaces
{    
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id);
        Task<IEnumerable<Room>> GetAllAsync();
        Task<IEnumerable<Room>> GetByRoomTypeIdAsync(Guid roomTypeId);
    }
}
