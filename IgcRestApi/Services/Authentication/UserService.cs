using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace IgcRestApi.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IConfigurationService _configurationService;

        // inject database for user validation
        public UserService(ILogger<UserService> logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public bool IsValidUser(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // --- Check against  user Database
            // TODO: This should be checking against a user database or directory
            var authOk = (userName == _configurationService.ApiDefaultLogin &&
                          password == _configurationService.ApiDefaultPassword);

            return authOk;
        }
    }
}
