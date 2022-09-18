using Blog.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace Blog.MVC.Filters;

public class MvcExceptionFilter : ExceptionFilterAttribute
{
    private readonly IModelMetadataProvider _provider;
    public MvcExceptionFilter(IModelMetadataProvider provider)
    {
        _provider = provider;
    }
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationException:
                {
                    foreach (var error in validationException.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    context.Result = new ViewResult() { ViewData = new ViewDataDictionary(_provider, context.ModelState) };
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
                    context.Result = new ViewResult()
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                        {
                            Model = context.Exception.Message
                        }
                    };
                    break;
                }
        }

        base.OnException(context);
    }
}