using Identity.Microservice.Common.Interfaces.Services;
using Identity.Microservice.Common.ResponseModels.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
    {
        private readonly IAuthService _authService;
        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;            
        }
        public Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request.Email, request.Password);
        }
    }
}
