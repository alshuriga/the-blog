using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Web.Exceptions;

namespace MiniBlog.Web.Extensions;

public static class CheckAdminPageExtension
{
    public static void ValidateAdminAuth(this PageModel model, string? returnUrl = null)
    {
            if (!model.User.Identity?.IsAuthenticated ?? true)
            {
                throw new NotLoggedInException(returnUrl: returnUrl);
            }
            else if (!(model.User.IsInRole(RolesConstants.ADMIN_ROLE)))
            {
                throw new AccessDeniedException(returnUrl: returnUrl);
            }
        
    }
}
