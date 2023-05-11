﻿using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;

namespace eShop.AdminApp.Service
{
    public interface IUserClientApi
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUsersPagingRequest request);
        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);
        Task<ApiResult<bool>> UpdateUser(UpdateUserRequest request);
        Task<ApiResult<UserVm>> GetUserById(Guid Id);
    }
}