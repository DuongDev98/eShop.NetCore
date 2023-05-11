namespace eShop.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] validationErrors { get; set; }

        public ApiErrorResult()
        {
            success = false;
        }

        public ApiErrorResult(string message)
        {
            success = false;
            this.message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            success = false;
            this.validationErrors = validationErrors;
        }
    }
}