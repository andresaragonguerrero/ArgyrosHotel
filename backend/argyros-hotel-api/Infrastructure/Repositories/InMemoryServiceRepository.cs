using argyros_hotel_api.Aplication.Interfaces;
using argyros_hotel_api.Domain.Hotel;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryServiceRepository : IServiceRepository
    {
        private readonly List<Service> _services;

        public InMemoryServiceRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _services = new List<Service>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _services = JsonSerializer.Deserialize<List<Service>>(json, options)
                            ?? new List<Service>();
            }
        }

        public Task<Service?> GetByIdAsync(Guid id) =>
            Task.FromResult(_services.FirstOrDefault(s => s.Id == id));

        public Task<IEnumerable<Service>> GetAllAsync() =>
            Task.FromResult(_services.AsEnumerable());
    }
}
