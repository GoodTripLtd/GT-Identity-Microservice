using MediatR;

namespace Identity.Microservice.AppCore.Commands.RegisterUser
{
    public record RegisterUserCommand(string FirstName,
        string LastName,
        string Email,
        string Username,
        string Password) : IRequest<bool>;
}
