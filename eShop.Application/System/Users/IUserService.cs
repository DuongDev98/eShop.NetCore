﻿using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUsersPagingRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(UpdateUserRequest request);
        Task<ApiResult<UserVm>> GetById(Guid Id);
    }
}