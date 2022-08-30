namespace MiniBlog.Web.ViewModels;

public class AppErrorViewModel
{
    public string Message { get; set; } = "There's no additional data, sadly";
    public string ReturnUrl { get; set; } = "/";
}