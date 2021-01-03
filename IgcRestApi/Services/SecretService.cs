using Google.Cloud.SecretManager.V1;
using IgcRestApi.Services.Interfaces;
using Microsoft.Extensions.Logging;


namespace IgcRestApi.Services
{
    public class SecretService : ISecretService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationService _configuration;

        public SecretService(ILoggerFactory loggerFactory, IConfigurationService configuration)
        {
            _logger = loggerFactory.CreateLogger<StorageService>();
            _configuration = configuration;
        }

        /// <summary>
        /// GetSecretValue
        /// Retrieve latest secret value
        /// </summary>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public string GetSecretValue(string secretKey)
        {
            // Create the client.
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            // Build the resource name.
            SecretVersionName secretVersionName = new SecretVersionName(_configuration.GcpProjectId, secretKey, "latest");

            // Call the API.
            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            // Convert the payload to a string. Payloads are bytes by default.
            string  payload = result.Payload.Data.ToStringUtf8();
            return payload;
        }


    }
}
