namespace IgcRestApi.Services.Interfaces
{
    public interface ISecretService
    {
        string GetSecretValue(string secretKey);
    }
}