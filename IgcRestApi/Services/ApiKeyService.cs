using IgcRestApi.Services.Interfaces;

namespace IgcRestApi.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private const int MinimumApiKeyLength = 8;          // Minimial apiKey length for partial key

        private readonly IConfigurationService _configuration;
        private readonly ISecretService _secretService;

        private static string _publicApiKey = null;
        private static string _privateApiKey = null;

        public ApiKeyService(IConfigurationService configurationService,
                            ISecretService secretService)
        {
            _configuration = configurationService;
            _secretService = secretService;
        }

        public bool IsAuthorized(string apiKeyValue)
        {
            // Retrieve keys if not already done
            if (_publicApiKey==null || _privateApiKey == null)
            {
                _publicApiKey = _secretService.GetSecretValue(_configuration.GcpSecretKeyIgcRestApiPubliclApiKey);
                _privateApiKey = _secretService.GetSecretValue(_configuration.GcpSecretKeyIgcRestApiInternalApiKey);
            }

            var isApiKeyOK = apiKeyValue.Length >= MinimumApiKeyLength &&
                            (_publicApiKey.StartsWith(apiKeyValue) || _privateApiKey.StartsWith(apiKeyValue));

            return isApiKeyOK;
        }
    }
}
