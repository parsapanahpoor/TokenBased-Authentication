using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Application.Utilities.Extensions;
using TokenBased_Authentication.Domain.IRepositories.Role;
using EquipmentManagement.Presentation.HttpManager;

namespace TokenBased_Authentication.Presentation.Areas.Admin.ActionFilterAttributes;
public class CheckUserHasAnyRole : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var service = (IRoleQueryRepository)context.HttpContext.RequestServices.GetService(typeof(IRoleQueryRepository))!;

        var hasUserAnyRole = service.IsUser_Admin(context.HttpContext.User.GetUserId(), default).Result;

        if (!hasUserAnyRole)
        {
            context.Result = JsonResponseStatus.NotPermission();
        }
    }
}
