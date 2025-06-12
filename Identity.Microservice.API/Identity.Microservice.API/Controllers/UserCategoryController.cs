using MediatR;

namespace Identity.Microservice.API.Controllers
{
    public class UserCategoryController : BaseController
    {
        public UserCategoryController(IMediator mediator) : base(mediator)
        {
        }
    }
}
