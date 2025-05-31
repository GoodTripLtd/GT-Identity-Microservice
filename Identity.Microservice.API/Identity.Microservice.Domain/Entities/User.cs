using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
