using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScholarshipManagementAPI.DTOs.Common.Auth;
using ScholarshipManagementAPI.Helper.Utilities;

namespace ScholarshipManagementAPI.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase,IAsyncActionFilter
    {
        
        protected LoggedInUserDto CurrentUser { get; private set; } = null!;

        [NonAction]
        public async Task OnActionExecutionAsync(
       ActionExecutingContext context,
       ActionExecutionDelegate next)
        {
            // Skip for [AllowAnonymous]
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(x => x is AllowAnonymousAttribute);

            if (!allowAnonymous)
            {
                var currentUserContextService =
                    context.HttpContext.RequestServices
                           .GetRequiredService<CurrentUserContextService>();

                CurrentUser = await currentUserContextService.GetCurrentUserAsync();
            }

            await next();
        }

    }
}
