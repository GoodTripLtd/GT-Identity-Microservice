using Identity.Microservice.API.Models.Response;
using Identity.Microservice.AppCore.Commands;
using Identity.Microservice.AppCore.Queries;
using Identity.Microservice.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Microservice.API.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers(string? searchString, UserSearchType userSearchType)
        {
            var response = _mediator.Send(new GetAllUsersQuery(searchString, userSearchType));
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = _mediator.Send(new GetUserByIdQuery(id));
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser()
        {
            return Ok();
        }

        [HttpPut("blockToggle")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BlockToggle(Guid guid)
        {
            var response = _mediator.Send(new BanUserCommand(guid));
            return Ok(response);
        }
    }
}
