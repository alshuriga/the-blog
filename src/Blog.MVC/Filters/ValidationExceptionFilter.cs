using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Blog.MVC.Filters;

public class ValidationExceptionFilter : ExceptionFilterAttribute
{
    private readonly IModelMetadataProvider _provider;
    public ValidationExceptionFilter(IModelMetadataProvider provider)
    {
        _provider = provider;
    }
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException valEx)
        {
            foreach (var error in valEx.Errors)
            {
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            context.Result = new ViewResult() { ViewData = new ViewDataDictionary(_provider, context.ModelState)};
            context.ExceptionHandled = true;
        }
        base.OnException(context);
    }
}