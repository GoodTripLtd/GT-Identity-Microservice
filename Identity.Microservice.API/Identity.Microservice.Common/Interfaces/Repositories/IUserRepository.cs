using Identity.Microservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<int> AddAsync(string sub, string firstName,
            string lastName,
            string email,
            string username);
        Task UpdateAsync(string firstName,
            string lastName,
            string email,
            string username);
        Task DeleteAsync(Guid id);
    }
}
