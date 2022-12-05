using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SAM.WEB.Services
{
    public class ActivityLoggerActionFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _accessor;

        public ActivityLoggerActionFilter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var appuserEmail = ControllerHelper.GetAppUserFromHttpContext(context.HttpContext);
            context.HttpContext.Items.Add("UserEmail", appuserEmail);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
