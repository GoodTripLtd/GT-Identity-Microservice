using MediatR;

namespace Identity.Microservice.AppCore.Commands.RegisterUser
{
    public record RegisterUserCommand() : IRequest<string>;
}
