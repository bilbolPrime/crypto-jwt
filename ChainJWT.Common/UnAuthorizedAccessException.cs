namespace BilbolStack.ChainJWT.Common
{
    public class UnAuthorizedAccessException : Exception
    {
        public UnAuthorizedAccessException() : base() { }

        public UnAuthorizedAccessException(string message) : base(message) { }
    }
}
