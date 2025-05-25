using MediatR;
using MLTEST.Interfaces.Services;

namespace MLTEST.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request.Data);

            return result;
        }
    }
}
