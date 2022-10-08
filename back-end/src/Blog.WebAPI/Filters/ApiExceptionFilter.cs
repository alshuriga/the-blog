using Blog.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Blog.API.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly IModelMetadataProvider _provider;
    public ApiExceptionFilter(IModelMetadataProvider provider)
    {
        _provider = provider;
    }
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationException:
                {
                    var valErrors = validationException.Errors.ToDictionary(v => v.PropertyName, v => v.ErrorMessage);
                    context.Result = new JsonResult(valErrors) { StatusCode = StatusCodes.Status400BadRequest };
                    context.ExceptionHandled = true;
                    break;
                }

            case NotFoundException notFoundException:
                {
                    context.Result = new NotFoundResult();
                    context.ExceptionHandled = true;
                    break;
                }
            default:
                {
                    var error = new ErrorDetails() { Message = context.Exception.Message };
                    context.Result = new JsonResult(error) { StatusCode = StatusCodes.Status400BadRequest };
                    context.ExceptionHandled = true;
                    break;
                }
        }

        base.OnException(context);
    }
}

public class ErrorDetails
{
        public string Message { get; set; } = null!;
        
}