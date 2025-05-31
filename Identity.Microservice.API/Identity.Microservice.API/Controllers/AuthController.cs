using Identity.Microservice.API.Controllers.Request;
using Identity.Microservice.API.Models.Response;
using Identity.Microservice.AppCore.Commands.RegisterUser;
using Identity.Microservice.AppCore.Commands.ConfirmUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Microservice.API.Models.Request;
using Identity.Microservice.AppCore.Commands.LoginUser;
using Identity.Microservice.AppCore.Commands.ChangePassword;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Microservice.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger) : base(mediator)
        {
            _logger = logger;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            var response = await _mediator.Send(new RegisterUserCommand(
                data.FirstName,
                data.LastName,
                data.Email,
                data.Username,
                data.Password));

            return Ok(response);
        }

        [HttpPost("ConfirmUser")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmUser(ConfirmUserRequest data)
        {
            var response = await _mediator.Send(new ConfirmUserCommand(data.Email, data.Password, data.Code));

            return Ok(response);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserRequest data)
        {
            var response = await _mediator.Send(new LoginUserCommand(data.Email, data.Password));

            return Ok(response);
        }

        [HttpPost("ChangePassword")]
        [Authorize("user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest data)
        {
            await _mediator.Send(new ChangePasswordCommand(data.AccessToken, data.PreviousPassword, data.ProposedPassword));

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser()
        {
            return Ok();
        }
    }
}
