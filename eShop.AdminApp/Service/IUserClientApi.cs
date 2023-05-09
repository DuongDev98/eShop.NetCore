using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;

namespace eShop.AdminApp.Service
{
    public interface IUserClientApi
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<UserVm>> GetUsersPaging(GetUsersPagingRequest request);
        Task<bool> RegisterUser(RegisterRequest request);
    }
}