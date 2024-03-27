using Microsoft.AspNetCore.Mvc;

namespace Employee_timesheet.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {

                context.Result = RedirectToAction("Login", "UserLogin");        // preventing the original action from being executed 
            }

            base.OnActionExecuting(context);
        }
    }
}
