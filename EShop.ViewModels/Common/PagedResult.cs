namespace eShop.ViewModels.Common
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Items = new List<T>();
            TotalRecord = 0;
        }
        public List<T> Items { set; get; }
        public int TotalRecord { get; set; }
    }
}
