using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniBlog.Web.Controllers;
using MiniBlog.Web.ViewModels;

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
       // if (!_hostEnvironment.IsDevelopment())
       // {
            if (context.Exception.GetType() == typeof(MiniBlogWebException))
            {
                context.Result = new ViewResult()
                {
                    ViewName = "_AppError", 
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = new AppErrorViewModel() { Message = context.Exception.Message, ReturnUrl = (context.Exception as MiniBlogWebException)?.ReturnUrl ?? "/"}
                    }
                };
            }
        //}
    } 
}