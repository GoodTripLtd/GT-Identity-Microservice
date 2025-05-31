using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Якщо треба: .AsNoTracking() для «легкого» завантаження, коли ми не плануємо змінювати об’єкти
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<int> AddAsync(string sub, string firstName,
        string lastName,
        string email,
        string username)
        {
            var user = new User
            {
                Id = new Guid(sub),
                Firstname = firstName,
                LastName = lastName,
                Email = email,
                Username = username,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Тут Id і CreatedAt уже мають бути заповнені, але можна перевірити/ініціювати за потреби
            await _dbContext.Users.AddAsync(user);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(string firstName,
        string lastName,
        string email,
        string username)
        {
            var user = new User
            {
                Firstname = firstName,
                LastName = lastName,
                Email = email,
                Username = username
            };
            _dbContext.Users.Update(user);
            user.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _dbContext.Users.FindAsync(id);
            if (existing == null)
                return; // або кинути помилку, якщо потрібно

            _dbContext.Users.Remove(existing);
            await _dbContext.SaveChangesAsync();
        }
    }
}
