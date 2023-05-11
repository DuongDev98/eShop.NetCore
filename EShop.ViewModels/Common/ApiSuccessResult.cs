namespace eShop.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult()
        {
            success = true;
            message = "";
        }

        public ApiSuccessResult(T data)
        {
            success = true;
            message = "";
            this.data = data;
        }
    }
}
