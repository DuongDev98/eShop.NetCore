﻿using eShop.ViewModels.Common;
using eShop.ViewModels.System.Users;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PagedResult<UserVm>> GetUsersPaging(GetUsersPagingRequest request);
    }
}