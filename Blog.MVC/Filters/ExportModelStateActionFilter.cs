using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Blog.MVC.Filters
{
    public class ExportModelStateActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid &&
                (context.Result is RedirectResult || context.Result is RedirectToActionResult || context.Result is RedirectToRouteResult))
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    var errors = controller.ModelState.ToDictionary(m => m.Key, m => m.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>());
                    controller.TempData["ModelState"] = JsonConvert.SerializeObject(errors);
                }

            }
            base.OnActionExecuted(context);
        }
    }

    public class ImportModelStateActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.Result is ViewResult result && result.TempData.ContainsKey("ModelState"))
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(controller.TempData["ModelState"] as string);
                    foreach(var errorList in errors)
                    {
                        foreach(var error in errorList.Value)
                        {
                            controller.ModelState.AddModelError(errorList.Key, error);
                        }
                    }
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
