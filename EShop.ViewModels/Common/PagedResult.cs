namespace eShop.ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public PagedResult()
        {
            Items = new List<T>();
        }
        public List<T> Items { set; get; }
    }
}
