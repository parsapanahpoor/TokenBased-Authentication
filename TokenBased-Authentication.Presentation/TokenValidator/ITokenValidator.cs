using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TokenBased_Authentication.Application.Utilities.Security;
using TokenBased_Authentication.Domain.IRepositories.User;
using TokenBased_Authentication.Presentation.Controllers.v1;
using TokenBased_Authentication.Presentation.Filter;

namespace TokenBased_Authentication.Presentation.TokenValidator;

public interface ITokenValidator
{
    Task Execute(TokenValidatedContext context , CancellationToken cancellationToken =default);
}

public class TokenValidate : ITokenValidator 
{
    #region ctor

    private readonly IUserQueryRepository _userQueryRepository;

    public TokenValidate(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    #endregion

    public async Task Execute(TokenValidatedContext context , CancellationToken cancellationToken=default)
    {
        //User is not logged in 
        var claimsidentity = context.Principal.Identity as ClaimsIdentity;
        if (claimsidentity?.Claims == null || !claimsidentity.Claims.Any())
        {
            context.Fail("claims not found....");
            return;
        }

        //User is not exist
        var userId = claimsidentity.FindFirst("NameIdentifier").Value;
        if (!ulong.TryParse(userId, out ulong userGuid))
        {
            context.Fail("claims not found....");
            return;
        }

        //Get User By Id 
        var user = await _userQueryRepository.GetByIdAsync(cancellationToken , userGuid);

        //User Is not active
        if (user.IsActive == false)
        {
            context.Fail("User not Active");
            return;
        }

        //User Token is not exist
        if (!(context.SecurityToken is JwtSecurityToken Token)
               || !await  _userQueryRepository.CheckIsExist_UserToken(Token.RawData))
        {
            context.Fail("توکد در دیتابیس وجود ندارد");
            return;
        }
    }
}
