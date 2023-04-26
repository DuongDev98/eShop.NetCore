using eShop.ViewModels.System.Users;

namespace eShop.AdminApp.Service
{
    public interface IUserClientApi
    {
        Task<string> Authenticate(LoginRequest request);
    }
}