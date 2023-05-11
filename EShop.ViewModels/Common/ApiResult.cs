namespace eShop.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool success { set; get; }
        public string message { set; get; }
        public T data { set; get; }
    }
}