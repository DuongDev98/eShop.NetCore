using Azure.Core;
using eShop.AdminApp.Service.Common;
using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using eShop.ViewModels.System.Users;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace eShop.AdminApp.Service.User
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor) { }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            return await PostAsync<string>("users/authenticate", request);
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetAll(GetUsersRequest request)
        {
            return await GetAsync<PagedResult<UserVm>>($"users?pageIndex={request.pageIndex}&pageSize={request.pageSize}&keyword={request.keyword}");
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            return await PostAsync<bool>("users", request);
        }

        public async Task<ApiResult<bool>> UpdateUser(UserUpdateRequest request)
        {
            return await PutAsync<bool>("users/" + request.Id.ToString(), request);
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid Id)
        {
            return await DeleteAsync<bool>("users/" + Id.ToString());
        }

        public async Task<ApiResult<UserVm>> GetUserById(Guid Id)
        {
            return await GetAsync<UserVm>("users/" + Id.ToString());
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            return await PutAsync<bool>($"users/{Id}/roles", request);
        }
    }
}
