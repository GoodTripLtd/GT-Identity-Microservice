using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Domain.Entities
{
    public class UserCategory : BaseEntity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
