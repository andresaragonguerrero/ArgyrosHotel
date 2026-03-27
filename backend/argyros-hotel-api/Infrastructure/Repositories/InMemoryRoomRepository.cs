using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Hotel;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryRoomRepository : IRoomRepository
        {
            private readonly List<Room> _rooms;

            public InMemoryRoomRepository(string jsonFilePath)
            {
                if (!File.Exists(jsonFilePath))
                    _rooms = new List<Room>();
                else
                {
                    var json = File.ReadAllText(jsonFilePath);
                    _rooms = JsonSerializer.Deserialize<List<Room>>(json) 
                             ?? new List<Room>();
                }
            }

            public Task<Room?> GetByIdAsync(Guid id) => 
                Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));

            public Task<IEnumerable<Room>> GetAllAsync() =>
                Task.FromResult(_rooms.AsEnumerable());

            public Task<IEnumerable<Room>> GetByRoomTypeIdAsync(Guid roomTypeId)
            {
                var rooms = _rooms.Where(r => r.RoomTypeId == roomTypeId);
                return Task.FromResult(rooms.AsEnumerable());
            }
        }
}
