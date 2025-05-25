using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using AutoMapper;
using Microsoft.Extensions.Options;
using MLTEST.Interfaces.Repositories;
using MLTEST.Interfaces.Services;
using MLTEST.Models.Entities;
using MLTEST.Models.Exceptions;
using MLTEST.Models.Options;
using MLTEST.Models.Request;
using MLTEST.Models.Response;
using System.Net;

namespace Identity.Microservice.AppCore.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly CognitoUserPool _userPool;
        private readonly IUserRepository _userRepository;
        private readonly AmazonCognitoOptions _cognitoConfig;
        private readonly IMapper _mapper;

        public AuthService(IAmazonCognitoIdentityProvider identityProvider, CognitoUserPool userPool, IUserRepository userRepository
            , IOptions<AmazonCognitoOptions> options, IMapper mapper)
        {
            _identityProvider = identityProvider;
            _userPool = userPool;
            _userRepository = userRepository;
            _cognitoConfig = options.Value;
            _mapper = mapper;
        }

        public async Task<LoginUserView> LoginAsync(LoginUserRequest data)
        {
            var user = new CognitoUser(data.Email, _cognitoConfig.ClientId, _userPool, _identityProvider);
            var authRequest = new InitiateSrpAuthRequest
            {
                Password = data.Password
            };

            var authResponse = await user.StartWithSrpAuthAsync(authRequest);

            var expiresAt = DateTime.Now + TimeSpan.FromSeconds(authResponse.AuthenticationResult.ExpiresIn);

            var response = new LoginUserView
            {
                IdToken = authResponse.AuthenticationResult.IdToken,
                RefreshToken = authResponse.AuthenticationResult.RefreshToken,
                ExpiresAt = expiresAt
            };

            return response;
        }

        public async Task<string> RegisterAsync(RegisterUserRequest data)
        {
            bool isAdded = false;

            var request = new SignUpRequest
            {
                ClientId = _cognitoConfig.ClientId,
                Password = data.Password,
                Username = data.Username,
                UserAttributes = new List<AttributeType>
                {
                    new()
                    {
                        Name = "email",
                        Value = data.Email
                    },
                    new()
                    {
                        Name = "custom:role",
                        Value = "user"
                    },
                    new()
                    {
                        Name = "name",
                        Value = data.Username
                    }
                },
            };

            var signUpResponse = await _identityProvider.SignUpAsync(request);

            if (signUpResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var user = _mapper.Map<UserEntity>(data);
                var role = await _userRepository.GetRoleByTitleAsync("user");

                if (role is null) 
                {
                    var deleteRequest = new AdminDeleteUserRequest
                    {
                        Username = data.Username,
                        UserPoolId = _cognitoConfig.UserPoolId
                    };

                    await _identityProvider.AdminDeleteUserAsync(deleteRequest);
                    throw new HttpException("Role hasn't been found", HttpStatusCode.NotFound);
                }

                user.Role = role;

                try
                {
                    await _userRepository.AddUserAsync(user);
                    isAdded = true;
                }
                catch
                {
                    var deleteRequest = new AdminDeleteUserRequest
                    {
                        Username = data.Username,
                        UserPoolId = _cognitoConfig.UserPoolId
                    };

                    await _identityProvider.AdminDeleteUserAsync(deleteRequest);
                }
            }

            return isAdded ? $"User {data.Username} has been successfully registered. You need to verify you email now." : "User hasn't been registered.";
        }
    }
}
