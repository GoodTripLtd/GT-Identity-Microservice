using Identity.Microservice.Common.ResponseModels.User;
using Identity.Microservice.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Identity.Microservice.API.Mappers
{
    [Mapper]
    public static partial class UserMapper
    {
        public static partial UserResponseDto EntityToDto(this User user);
    }
}
