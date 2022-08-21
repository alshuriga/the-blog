namespace MiniBlog.Web.Exceptions;

public class MiniBlogWebException : ApplicationException
{
    public string ReturnUrl { get; private set; } = "/";
    public MiniBlogWebException(string? message, string? returnUrl = null): base(message)
    {
        if(returnUrl != null) ReturnUrl = returnUrl;
    }
}