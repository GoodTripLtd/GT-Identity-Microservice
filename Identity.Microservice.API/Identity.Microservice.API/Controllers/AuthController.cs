using Identity.Microservice.API.Controllers.Request;
using Identity.Microservice.API.Models.Response;
using Identity.Microservice.AppCore.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserRequest data)
        {
            var response = await _mediator.Send(new RegisterUserCommand());

            return Ok(response);
        }
    }
}
