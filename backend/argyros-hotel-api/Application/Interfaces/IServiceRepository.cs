using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Application.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(Guid id);
        Task<IEnumerable<Service>> GetAllAsync();
    }
}
