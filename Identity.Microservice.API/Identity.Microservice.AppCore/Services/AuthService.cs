using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Azure.Core;
using Identity.Microservice.API.Mappers;
using Identity.Microservice.AppCore.Exceptions;
using Identity.Microservice.AppCore.Options;
using Identity.Microservice.Common.Interfaces.Repositories;
using Identity.Microservice.Common.Interfaces.Services;
using Identity.Microservice.Common.ResponseModels.Auth;
using Microsoft.Extensions.Options;
using System.Net;

namespace Identity.Microservice.AppCore.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly CognitoUserPool _userPool;
        private readonly AmazonCognitoOptions _cognitoConfig;

        public AuthService(IAmazonCognitoIdentityProvider identityProvider,
            CognitoUserPool userPool,
            IOptions<AmazonCognitoOptions> options)
        {
            _identityProvider = identityProvider;
            _userPool = userPool;
            _cognitoConfig = options.Value;
        }

        public async Task<bool> ConfirmSignUpAsync(string username, string confirmationCode)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username не може бути порожнім", nameof(username));

            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentException("Confirmation code не може бути порожнім", nameof(confirmationCode));

            var confirmRequest = new ConfirmSignUpRequest
            {
                ClientId = _cognitoConfig.ClientId,
                Username = username,
                ConfirmationCode = confirmationCode
            };

            var response = await _identityProvider.ConfirmSignUpAsync(confirmRequest);

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Неочікуваний статус від Cognito: {response.HttpStatusCode}");
            }

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {
            var user = new CognitoUser(email, _cognitoConfig.ClientId, _userPool, _identityProvider);
            var authRequest = new InitiateSrpAuthRequest
            {
                Password = password
            };

            var authResponse = await user.StartWithSrpAuthAsync(authRequest);

            var expiresAt = DateTime.Now + TimeSpan.FromSeconds(Convert.ToDouble(
                authResponse.AuthenticationResult.ExpiresIn));

            var response = new LoginResponseDto
            {
                IdToken = authResponse.AuthenticationResult.IdToken,
                RefreshToken = authResponse.AuthenticationResult.RefreshToken,
                AccessToken = authResponse.AuthenticationResult.AccessToken,
                ExpiresAt = expiresAt
            };

            return response;
        }

        public async Task<string?> RegisterAsync(string firstName,
            string lastName,
            string email,
            string username,
            string password)
        {
            bool isAdded = false;

            var request = new SignUpRequest
            {
                ClientId = _cognitoConfig.ClientId,
                Password = password,
                Username = email,
                UserAttributes = new List<AttributeType>
                {
                    new()
                    {
                        Name = "email",
                        Value = email
                    },
                    new()
                    {
                        Name = "custom:role",
                        Value = "user"
                    },
                    new()
                    {
                        Name = "name",
                        Value = username
                    }
                },
            };

            var signUpResponse = await _identityProvider.SignUpAsync(request);

            if (signUpResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                return signUpResponse.UserSub;
            }
            return null;
        }

        public async Task ChangePasswordAsync(string accessToken, string previousPassword, string proposedPassword)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("AccessToken не може бути порожнім.", nameof(accessToken));

            if (string.IsNullOrWhiteSpace(previousPassword))
                throw new ArgumentException("Попередній пароль не може бути порожнім.", nameof(previousPassword));

            if (string.IsNullOrWhiteSpace(proposedPassword))
                throw new ArgumentException("Новий пароль не може бути порожнім.", nameof(proposedPassword));

            var request = new ChangePasswordRequest
            {
                AccessToken = accessToken,
                PreviousPassword = previousPassword,
                ProposedPassword = proposedPassword
            };
            new ConfirmForgotPasswordRequest { };
            var response = await _identityProvider.ChangePasswordAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Неочікуваний статус при зміні паролю: {response.HttpStatusCode}");
            }
        }
        public async Task ForgotPasswordAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username не може бути порожнім.", nameof(username));

            var request = new ForgotPasswordRequest
            {
                ClientId = _cognitoConfig.ClientId,
                Username = username
            };

            try
            {
                var response = await _identityProvider.ForgotPasswordAsync(request);

            }
            catch (UserNotFoundException)
            {
                throw new InvalidOperationException("Користувача з таким ім’ям не знайдено.");
            }
            catch (LimitExceededException)
            {
                throw new InvalidOperationException("Перевищено ліміт запитів на відновлення паролю. Спробуйте пізніше.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при ініціалізації відновлення паролю: {ex.Message}", ex);
            }
        }
        public async Task ConfirmForgotPasswordAsync(string username, string confirmationCode, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username не може бути порожнім.", nameof(username));
            if (string.IsNullOrWhiteSpace(confirmationCode))
                throw new ArgumentException("ConfirmationCode не може бути порожнім.", nameof(confirmationCode));
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Новий пароль не може бути порожнім.", nameof(newPassword));

            var request = new ConfirmForgotPasswordRequest
            {
                ClientId = _cognitoConfig.ClientId,
                Username = username,
                ConfirmationCode = confirmationCode,
                Password = newPassword
            };

            try
            {
                var response = await _identityProvider.ConfirmForgotPasswordAsync(request);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Неочікуваний статус: {response.HttpStatusCode}");
                }
            }
            catch (UserNotFoundException)
            {
                throw new InvalidOperationException("Користувача з таким ім’ям не знайдено.");
            }
            catch (CodeMismatchException)
            {
                throw new InvalidOperationException("Невірний код підтвердження.");
            }
            catch (ExpiredCodeException)
            {
                throw new InvalidOperationException("Код підтвердження прострочений. Спробуйте запросити новий.");
            }
            catch (InvalidPasswordException ex)
            {
                throw new InvalidOperationException($"Новий пароль не відповідає вимогам: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка при підтвердженні відновлення паролю: {ex.Message}", ex);
            }
        }
    }
}
