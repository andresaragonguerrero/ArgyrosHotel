using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Users;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    // Implementación básica en memoria del repositorio de usuarios
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        // Se carga la lista de usuarios desde un archivo JSON al crear la instancia del repositorio
        public InMemoryUserRepository(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                _users = new List<User>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                _users = JsonSerializer.Deserialize<List<User>>(json)
                         ?? new List<User>();
            }
        }

        //public InMemoryUserRepository(List<User>? initialUsers = null)
        //{
        //    _users = initialUsers ?? new List<User>();
        //}

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                _users.Remove(user);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(_users.AsEnumerable());

        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));

        public Task<User?> GetByIdAsync(Guid id) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

        public Task UpdateAsync(User user)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
                _users[index] = user;
            return Task.CompletedTask;
        }
    }
}