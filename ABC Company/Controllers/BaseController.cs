using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ABC_Company.Controllers
{
    public class BaseController : Controller
    {

        protected string UserIdClaim { get; private set; }
        protected string UserNameClaim { get; private set; }
        protected string EmailClaim { get; private set; }
        protected string RoleClaim { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity.IsAuthenticated)
            {
                UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                UserNameClaim = User.Identity.Name;
                EmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                RoleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

				ViewBag.UserIdClaim = UserIdClaim;
				ViewBag.UserNameClaim = UserNameClaim;
				ViewBag.EmailClaim = EmailClaim;
				ViewBag.RoleClaim = RoleClaim;
			}
            else
            {
                UserNameClaim = "Guest";
                UserIdClaim = null;
                EmailClaim = null;
                RoleClaim = null;
            }
        }
    }
}
