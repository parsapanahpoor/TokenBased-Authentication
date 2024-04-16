using EquipmentManagement.Presentation.HttpManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.Login;
using TokenBased_Authentication.Domain.DTO.APIClient.Account;
using TokenBased_Authentication.Domain.Entities.Account;
using TokenBased_Authentication.Application.Utilities.Security;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.CreateToken;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.SendSMSCode;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.FindRefreshToken;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Query.CreateToken;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.DeleteToken;
using Microsoft.AspNetCore.Authorization;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.Account.Command.LogOut;
using TokenBased_Authentication.Application.Utilities.Extensions;

namespace TokenBased_Authentication.Presentation.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]

public class AccountController : SiteBaseController
{
    #region Ctor

    private readonly IConfiguration _configuration;

    public AccountController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Send SMS Code 

    [HttpPost("SendSMSCode")]
    public async Task<IActionResult> SendSMSCode(string phoneNumber,
                                                 CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new SendSMSCodeCommand()
        {
            PhoneNumber = phoneNumber,
        },
        cancellationToken);

        return Ok(JsonResponseStatus.Success(res, "کدفعال سازی ارسال شده است."));
    }

    #endregion

    #region Register Or Login 

    [HttpPost("Login_Register")]
    public async Task<IActionResult> Login_Register([FromBody] LoginQuery query,
                                                    CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(query, cancellationToken);

        //If Got any problem with sms code
        if (res == null || res.IsSuccess == false) return Ok(JsonResponseStatus.Success(new
        {
            IsSuccess = false
        },
        res.Message));

        //If sms code was ok , then user will login or register
        var token = await CreateToken(res.User.Id, cancellationToken);

        return Ok(new LoginRegisterResultDTO()
        {
            IsSuccess = true,
            Data = token,
        });
    }

    #endregion

    #region Refresh Token

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(string Refreshtoken,
                                                  CancellationToken cancellationToken)
    {
        var usertoken = await Mediator.Send(new FindRefreshTokenQuery() { RefreshToken = Refreshtoken });

        //If Token is not exist
        if (usertoken == null) return Unauthorized();

        //If refresh token is expired
        if (usertoken.TokenExpireTime < DateTime.Now) return Unauthorized("Token Expire");

        //Create New Token
        var token = await CreateToken(usertoken.UserId, cancellationToken);

        //Delete Lastest Token
        //await Mediator.Send(new DeleteTokenCommand() { RefreshTokenId = usertoken.RefreshTokenId });

        return Ok(JsonResponseStatus.Success(token, "توکن کاربر باموفقیت بازیابی شده است."));
    }

    #endregion

    #region Create Token And Refresh Token

    private async Task<LoginDataDto> CreateToken(ulong userId,
                                                 CancellationToken cancellationToken)
    {
        //Get User By Id 
        var user = await Mediator.Send(new CreateTokenQuery() { UserId = userId });

        var claims = new List<Claim>
                {
                    new Claim("NameIdentifier", user.Id.ToString()),
                    new Claim("MobilePhone", user.Mobile),
                    new Claim ("Name",  user?.Username ?? ""),
                };

        //A Key For Hashing
        string key = _configuration["JWtConfig:Key"];

        //Token expire time
        var tokenexp = DateTime.Now.AddDays(int.Parse(_configuration["JWtConfig:expires"]));

        //Create new JWT Token 
        var token = new JwtSecurityToken(
            issuer: _configuration["JWtConfig:issuer"],//Company or WebSite Name that raised from appsetting
            audience: _configuration["JWtConfig:audience"],//Company or WebSite Name that raised from appsetting
            expires: tokenexp,//Token expire time
            notBefore: DateTime.Now,//When token is ready to use 
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                                                       SecurityAlgorithms.HmacSha256)//Hshing algorithm
            );

        //Encode upper token to usuall token format
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        //Save JWT Token Data in Database
        return await Mediator.Send(new CreateTokenCommand()
        {
            LastestSignInPlatformName = "Google Chrome",
            TokenExpireTime = tokenexp,
            TokenHash = jwtToken,
            UserId = user.Id,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpireTime = DateTime.Now.AddDays(30)
        },
        cancellationToken);
    }


    #endregion

    #region Log Out

    [Authorize]
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new LogOutCommand()
        {
            UserId = User.GetUserId(),
        });

        if (res)
        {
            return Ok(JsonResponseStatus.Success(null , "کاربر باموفقیت از سامانه خارج شده است."));
        }

        return NotFound();
    }

    #endregion
}
