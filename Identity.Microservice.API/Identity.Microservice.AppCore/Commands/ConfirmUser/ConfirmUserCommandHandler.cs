using Azure;
using Identity.Microservice.Common.Interfaces.Services;
using Identity.Microservice.Common.ResponseModels.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands.ConfirmUser
{
    public class ConfirmUserCommandHandler : IRequestHandler<ConfirmUserCommand, LoginResponseDto>
    {
        private readonly IAuthService _authService;
        public ConfirmUserCommandHandler(IAuthService authService)
        {
            _authService = authService;   
        }
        public async Task<LoginResponseDto> Handle(ConfirmUserCommand request, CancellationToken cancellationToken)
        {
            var confirmResult = await _authService.ConfirmSignUpAsync(request.Username, request.Code);
            if (confirmResult)
            {
                return await _authService.LoginAsync(request.Username, request.Password);
            }
            throw new Exception($"Невірний код");
        }
    }
}
