using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniBlog.Web.Controllers;
using MiniBlog.Web.ViewModels;
using MiniBlog.Web.Exceptions;

namespace MiniBlog.Web.Filters;

public class ApplicationExceptionFilter : IExceptionFilter
{

    private readonly IHostEnvironment _hostEnvironment;

    public ApplicationExceptionFilter(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }
    public void OnException(ExceptionContext context)
    {
        bool isGet = context.HttpContext.Request.Method == HttpMethod.Get.Method;
        string? url = isGet ? context.HttpContext.Request.Path.Value : null;

        if (context.Exception is NotLoggedInException noLogInEx)
        {
            context.Result = new RedirectToActionResult(nameof(AccountController.Login), "Account", new { ReturnUrl = url ?? noLogInEx.ReturnUrl });
        }
        else if (context.Exception is AccessDeniedException accessEx)
            context.Result = new RedirectToActionResult(nameof(AccountController.AccessDenied), "Account", new { ReturnUrl = url ?? accessEx.ReturnUrl }); 

        else if (context.Exception is MiniBlogWebException ex)
        {
            context.Result = new ViewResult()
            {
                ViewName = "_AppError",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = new AppErrorViewModel() { Message = context.Exception.Message, ReturnUrl = url ?? ex.ReturnUrl }
                }
            };
        }
       
        //}
    }
}