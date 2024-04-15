﻿using EquipmentManagement.Presentation.HttpManager;
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

    #region Register Or Login 

    [HttpPost]
    public async Task<IActionResult> Login_Register([FromBody] LoginQuery query,
                                                    CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(query, cancellationToken);

        //If Got any problem with sms code
        if (res == null || res.IsSuccess == false) return Ok(JsonResponseStatus.Success(new LoginRegisterResultDTO()
        {
            Message = res.Message,
            IsSuccess = false
        }));

        //If sms code was ok , then user will login or register
        var token = await CreateToken(res.User, cancellationToken);

        return Ok(new LoginRegisterResultDTO()
        {
            IsSuccess = true,
            Data = token,
        });
    }

    #endregion

    #region Create Token And Refresh Token

    private async Task<LoginDataDto> CreateToken(User user,
                                           CancellationToken cancellationToken)
    {
        SecurityHelper securityHelper = new SecurityHelper();

        var claims = new List<Claim>
                {
                    new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new (ClaimTypes.MobilePhone, user.Mobile),
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
}