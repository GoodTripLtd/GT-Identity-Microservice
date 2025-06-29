﻿using Identity.Microservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Common.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> BanToggle(Guid id);
    }
}
