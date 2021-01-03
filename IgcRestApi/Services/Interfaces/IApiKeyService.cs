namespace IgcRestApi.Services
{
    public interface IApiKeyService
    {
        bool IsAuthorized(string apiKeyValue);
    }
}