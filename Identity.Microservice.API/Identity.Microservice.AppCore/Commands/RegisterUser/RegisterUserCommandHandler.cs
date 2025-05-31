using Identity.Microservice.AppCore.Commands.RegisterUser;
using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Common.Interfaces.Services;
using MediatR;

namespace Identity.Microservice.AppCore.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userSub = await _authService.RegisterAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Username,
                request.Password);

            if (userSub is not null)
            {
                if (await _userRepository.AddAsync(userSub, request.FirstName,
                request.LastName,
                request.Email,
                request.Username) > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
