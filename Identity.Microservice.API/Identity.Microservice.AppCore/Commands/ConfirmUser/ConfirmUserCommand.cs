using Amazon.Runtime.Internal;
using Identity.Microservice.Common.ResponseModels.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Microservice.AppCore.Commands.ConfirmUser
{
    public record ConfirmUserCommand(string Username, string Password, string Code) : IRequest<LoginResponseDto>;
}
