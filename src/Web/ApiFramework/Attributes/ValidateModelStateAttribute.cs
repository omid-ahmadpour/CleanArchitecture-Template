using CleanTemplate.ApiFramework.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanTemplate.ApiFramework.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelStateEntries = context.ModelState.Values;

                foreach (var item in modelStateEntries)
                {
                    foreach (var error in item.Errors)
                    {
                        ApiResult resultObject = new ApiResult(error.ErrorMessage);
                        context.Result = new JsonResult(resultObject);
                    }
                }
            }
        }
    }
}
