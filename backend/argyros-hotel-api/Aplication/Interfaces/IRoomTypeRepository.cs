using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Aplication.Interfaces
{
    public interface IRoomTypeRepository
    {
        // Este repositorio se encarga de gestionar los tipos de habitaciones
        // EL PROYECTO NO REQUIERE DE FUNCIONALIDADES DE CREACIÓN, ACTUALIZACIÓN, ETC PORQUE NO ES UNA APLICACIÓN DE ADMINISTRACIÓN DE HOTEL
        public interface IRoomTypeRepository
        {
            Task<RoomType?> GetByIdAsync(Guid id);
            Task<IEnumerable<RoomType>> GetAllAsync();
        }
    }
}
