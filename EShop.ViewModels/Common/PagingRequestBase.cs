namespace eShop.ViewModels.Common
{
    public class PagingRequestBase : RequestBase
    {
        public int pageIndex { set; get; }
        public int pageSize { set; get; }
    }
}