using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Common.ResponseModels.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public string Firstname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
