using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands.ChangePassword
{
    public record ChangePasswordCommand(string AccessToken, string previousPassword, string proposedPassword) : IRequest;
}
