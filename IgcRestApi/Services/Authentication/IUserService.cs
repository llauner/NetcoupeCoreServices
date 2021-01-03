namespace IgcRestApi.Services.Authentication
{
    public interface IUserService
    {
        bool IsValidUser(string userName, string password);
    }
}