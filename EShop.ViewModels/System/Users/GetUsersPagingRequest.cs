using eShop.ViewModels.Common;

namespace eShop.ViewModels.System.Users
{
    public class GetUsersPagingRequest : PagingRequestBase
    {
        public string? keyword { set; get; }
    }
}
