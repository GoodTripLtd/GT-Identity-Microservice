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
    public record GetAllUserTagsQuery(Guid id) : IRequest<IEnumerable<Guid>>;

    public class GetAllUserTagsQueryHandler : IRequestHandler<GetAllUserTagsQuery, IEnumerable<Guid>>
    {
        private readonly IUserTagRepository _userTagRepository;

        public GetAllUserTagsQueryHandler(IUserTagRepository userTagRepository)
        {
            _userTagRepository = userTagRepository;
        }

        public async Task<IEnumerable<Guid>> Handle(GetAllUserTagsQuery request, CancellationToken cancellationToken)
        {
            return (await _userTagRepository.GetAllAsync()).Where(y => y.UserId == request.id).Select(x => x.TagId);

        }
    }
}
