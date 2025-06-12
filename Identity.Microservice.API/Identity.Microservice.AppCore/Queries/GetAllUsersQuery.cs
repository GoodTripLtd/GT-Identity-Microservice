using Identity.Microservice.API.Mappers;
using Identity.Microservice.Common.Enums;
using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Common.ResponseModels.User;
using Identity.Microservice.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Queries
{
    public record GetAllUsersQuery(string SearchString, UserSearchType UserSearchType) : IRequest<IEnumerable<UserResponseDto>>;

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetAllAsync();

            var filtered = request.UserSearchType switch
            {
                UserSearchType.Active => res.Where(u => !u.IsBlocked),
                UserSearchType.Blocked => res.Where(u => u.IsBlocked),
                UserSearchType.All => res,
                _ => res
            };

            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                var term = request.SearchString;
                filtered = filtered.Where(u =>
                    (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(term)) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(term)) ||
                    (!string.IsNullOrEmpty(u.Firstname) && u.Firstname.Contains(term)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.Contains(term))
                );
            }


            return filtered
                .Select(u => u.EntityToDto())
                .ToList();

        }
    }
}
