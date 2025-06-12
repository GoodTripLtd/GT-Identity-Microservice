using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Common.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands
{
    public record SendForgotPasswordCommand(string Username) : IRequest;

    public class SendForgotPasswordCommandHandler : IRequestHandler<SendForgotPasswordCommand>
    {
        private readonly IAuthService _authService;

        public SendForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task Handle(SendForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.ForgotPasswordAsync(request.Username);
        }
    }
}
