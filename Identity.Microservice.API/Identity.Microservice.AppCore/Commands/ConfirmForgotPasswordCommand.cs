using Identity.Microservice.Common.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands
{
    public record ConfirmForgotPasswordCommand(string Username, string Code, string NewPassword) : IRequest;
    public class ConfirmForgotPasswordCommandHandler : IRequestHandler<ConfirmForgotPasswordCommand>
    {
        private readonly IAuthService _authService;

        public ConfirmForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task Handle(ConfirmForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.ConfirmForgotPasswordAsync(request.Username, request.Code, request.NewPassword);
        }
    }
}
