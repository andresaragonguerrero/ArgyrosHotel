using ArgrosHotel.Application.Interfaces;
using ArgrosHotel.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgrosHotel.Infrastructure.Repositories
{
    // Implementación básica en memoria del repositorio de usuarios
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository(List<User>? initialUsers = null)
        {
            _users = initialUsers ?? new List<User>();
        }

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