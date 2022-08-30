namespace MiniBlog.Web.Exceptions
{
    public class NotLoggedInException : MiniBlogWebException
    {
        public NotLoggedInException(string? message = null, string? returnUrl = null) : base(message, returnUrl)
        {
        }
    }

    public class AccessDeniedException : MiniBlogWebException
    {
        public AccessDeniedException(string? message = null, string? returnUrl = null) : base(message, returnUrl)
        {
        }
    }
}
