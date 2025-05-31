using Identity.Microservice.Common.ResponseModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.Common.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> RegisterAsync(string firstName,
        string lastName,
        string email,
        string username,
        string password);

        public Task<LoginResponseDto> LoginAsync(string email, string password);

        public Task<bool> ConfirmSignUpAsync(string username, string code);
        public Task ChangePasswordAsync(string accessToken, string previousPassword, string proposedPassword);
    }
}
