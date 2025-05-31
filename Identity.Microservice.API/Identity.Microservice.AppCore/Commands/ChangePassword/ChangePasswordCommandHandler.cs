using Identity.Microservice.AppCore.Commands.ConfirmUser;
using Identity.Microservice.Common.Interfaces.Services;
using Identity.Microservice.Common.ResponseModels.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IAuthService _authService;
        public ChangePasswordCommandHandler(IAuthService authService)
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

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _authService.ChangePasswordAsync(request.AccessToken, request.previousPassword, request.proposedPassword);
        }
    }
}
