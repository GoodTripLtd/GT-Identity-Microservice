using Identity.Microservice.AppCore.Commands.RegisterUser;
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
    public record BanUserCommand(Guid id) : IRequest<bool>;
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public BanUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {

            return await _userRepository.BanToggle(request.id);
        }
    }
}
