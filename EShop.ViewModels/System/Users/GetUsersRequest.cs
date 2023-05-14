using eShop.ViewModels.Common;

namespace eShop.ViewModels.System.Users
{
    public class GetUsersRequest : PagingRequestBase
    {
        public string? keyword { set; get; }
    }
}
