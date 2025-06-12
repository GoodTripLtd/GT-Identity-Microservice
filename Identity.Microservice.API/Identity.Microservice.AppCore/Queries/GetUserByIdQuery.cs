using Identity.Microservice.API.Mappers;
using Identity.Microservice.Common.Enums;
using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Common.ResponseModels.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<UserResponseDto>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return (await _userRepository.GetByIdAsync(request.Id)).EntityToDto();
        }
    }
}
