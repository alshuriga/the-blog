﻿using Blog.Application.Exceptions;
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
                    var valErrors = new Dictionary<string, string>();
                    foreach (var error in validationException.Errors)
                    {
                        valErrors[string.IsNullOrEmpty(error.PropertyName) ? "_" : error.PropertyName] = valErrors.ContainsKey(error.PropertyName) ? valErrors[error.PropertyName] + $"\n{error.ErrorMessage}" : error.ErrorMessage;
                    }
                    context.Result = new JsonResult(valErrors) { StatusCode = StatusCodes.Status422UnprocessableEntity };
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