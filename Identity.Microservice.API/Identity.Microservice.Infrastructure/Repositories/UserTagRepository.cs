using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Infrastructure.Repositories
{
    public class UserTagRepository : GenericRepository<UserTag>, IUserTagRepository
    {
        public UserTagRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
