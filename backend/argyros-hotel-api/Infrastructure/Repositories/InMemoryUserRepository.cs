using argyros_hotel_api.Application.Interfaces;
using argyros_hotel_api.Domain.Users;
using System.Text.Json;

namespace argyros_hotel_api.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;
        private readonly string _jsonFilePath;

        public InMemoryUserRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;

            if (!File.Exists(jsonFilePath))
                _users = new List<User>();
            else
            {
                var json = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _users = JsonSerializer.Deserialize<List<User>>(json, options)
                         ?? new List<User>();
            }
        }

        public async Task AddAsync(User user)
        {
            _users.Add(user);
            await SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                await SaveAsync();
            }
        }

        public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(_users.AsEnumerable());

        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));

        public Task<User?> GetByIdAsync(Guid id) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

        public async Task UpdateAsync(User user)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            {
                _users[index] = user;
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_users, options);
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
    }
}