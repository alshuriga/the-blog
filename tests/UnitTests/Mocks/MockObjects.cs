using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Web.Pages;
using Microsoft.AspNetCore.Identity;
using MiniBlog.Tests.UnitTests.Mocks;
using MiniBlog.Web.ViewModels;
using Newtonsoft.Json;
using MiniBlog.Core.Constants;
using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MiniBlog.Core.Specifications;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;


namespace MiniBlog.Tests.UnitTests.Mocks;

public static class MockObjects
{
    public static PageContext GetPageContext(bool isLoggedIn, string userRole)
    {
        Claim[] Claims = { new Claim(ClaimTypes.Name, "User"),new Claim(ClaimTypes.Role, userRole), new Claim(ClaimTypes.Authentication, "true") };
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(Claims, isLoggedIn ? "TestAuth" : null));
        HttpContext httpContext = new DefaultHttpContext() { User = claimsPrincipal };
        ActionContext actionContext = new(httpContext, new RouteData(), new PageActionDescriptor());
        ViewDataDictionary viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        PageContext pageContext = new PageContext(actionContext) { ViewData = viewData};
        return pageContext;
    }
}
