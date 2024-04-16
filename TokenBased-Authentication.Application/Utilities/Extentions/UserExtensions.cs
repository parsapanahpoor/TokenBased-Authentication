using System.Security.Claims;
using System.Security.Principal;
namespace TokenBased_Authentication.Application.Utilities.Extensions;

public static class UserExtensions
{
    public static ulong GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var data = claimsPrincipal.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier);

        return ulong.Parse(data.Value);
    }

    public static ulong GetUserId(this IPrincipal principal)
    {
        var user = (ClaimsPrincipal)principal;

        return user.GetUserId();
    }
}
