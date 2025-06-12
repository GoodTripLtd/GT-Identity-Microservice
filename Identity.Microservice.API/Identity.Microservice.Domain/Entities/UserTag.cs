using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Domain.Entities
{
    public class UserTag : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
    }
}
