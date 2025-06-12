using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext):base(dbContext) { }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
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

        public async Task<bool> BanToggle(Guid id)
        {
            var rowsAffected = await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.IsBlocked, u => !u.IsBlocked));

            return rowsAffected > 0;
        }
    }
}
