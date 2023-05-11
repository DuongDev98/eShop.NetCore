namespace eShop.Utilities.Exceptions
{
    public class EShopException : Exception
    {
        public EShopException() { }
        public EShopException(string message) :base (message) { }
        public EShopException(string message, Exception inner) : base (message, inner) { }
    }
}
