using argyros_hotel_api.Domain.Hotel;
using argyros_hotel_api.Aplication.Interfaces;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    // Implementación básica en memoria del repositorio de tipos de habitación
    public class InMemoryRoomTypeRepository : IRoomTypeRepository
    {
        private readonly List<RoomType> _roomTypes;

        public InMemoryRoomTypeRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _roomTypes = new List<RoomType>();
            else
            {
                
                var json = File.ReadAllText(jsonFilePath);
                _roomTypes = JsonSerializer.Deserialize<List<RoomType>>(json) 
                                ?? new List<RoomType>();
                
            }
        }

        public Task<RoomType?> GetByIdAsync(Guid id) => 
            Task.FromResult(_roomTypes.FirstOrDefault(rt => rt.Id == id));

        public Task<IEnumerable<RoomType>> GetAllAsync() => 
            Task.FromResult(_roomTypes.AsEnumerable());
    }
}
