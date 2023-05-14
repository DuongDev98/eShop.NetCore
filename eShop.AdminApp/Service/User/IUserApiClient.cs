﻿using eShop.ViewModels.Common;
using eShop.ViewModels.System.Roles;
using eShop.ViewModels.System.Users;

namespace eShop.AdminApp.Service.User
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PagedResult<UserVm>>> GetAll(GetUsersRequest request);
        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);
        Task<ApiResult<bool>> UpdateUser(UserUpdateRequest request);
        Task<ApiResult<bool>> DeleteUser(Guid Id);
        Task<ApiResult<UserVm>> GetUserById(Guid Id);
        Task<ApiResult<bool>> RoleAssign(Guid Id, RoleAssignRequest request);
    }
}