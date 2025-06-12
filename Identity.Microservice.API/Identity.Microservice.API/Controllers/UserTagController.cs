using Identity.Microservice.API.Models.Response;
using Identity.Microservice.AppCore.Queries;
using Identity.Microservice.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Microservice.API.Controllers
{
    public class UserTagController : BaseController
    {
        public UserTagController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUserTags(Guid id)
        {
            var response = _mediator.Send(new GetAllUserTagsQuery(id));
            return Ok(response);
        }
    }
}
