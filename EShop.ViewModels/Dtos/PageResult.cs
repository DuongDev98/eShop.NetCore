namespace eShop.ViewModels.Dtos
{
    public class PageResult<T>
    {
        public List<T> Items { set; get; }
        public int TotalRecord { get; set; }
    }
}
