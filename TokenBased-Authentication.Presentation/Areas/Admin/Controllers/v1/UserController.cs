using EquipmentManagement.Presentation.HttpManager;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Command.EditUser;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Query;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.User.Query.UserDetailQuery;
using TokenBased_Authentication.Domain.DTO.AdminSide.User;

namespace TokenBased_Authentication.Presentation.Areas.Admin.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Admin/[controller]")]

public class UserController : AdminBaseController
{
    #region Filter Users 

    [HttpGet("FilterUsers")]
    public async Task<IActionResult> FilterUsers([FromQuery] FilterUsersAdminSideQuery filter,
                                                 CancellationToken cancellationToken = default)
    {
        return JsonResponseStatus.Success(await Mediator.Send(filter));
    }

    #endregion

    #region Edit User

    [HttpPut("EditUser")]
    public async Task<IActionResult> EditUser([FromForm]EditUserDTO userDTO,
                                              CancellationToken cancellation = default)
    {
        var res = await Mediator.Send(new EditUserAdminSideCommand()
        {
            Avatar = userDTO.UserAvatar,
            Id = userDTO.Id,
            IsActive = userDTO.IsActive,
            Mobile = userDTO.Mobile,
            Username = userDTO.Username,
            UserRoles = userDTO.UserRoles
        });

        switch (res)
        {
            case EditUserResult.DuplicateMobileNumber:
                return JsonResponseStatus.Error(null, "موبایل وارد شده تکراری می باشد.");

            case EditUserResult.Success:
                return JsonResponseStatus.Success(null, "ویرایش کابر موردنظر باموفقیت انجام شده است.");
        }

        return JsonResponseStatus.Error(null, "اطلاعات وارد شده صحیح نمی باشد.");
    }

    #endregion

    #region Show User Detail

    [HttpGet("UserDetail")]
    public async Task<IActionResult> UserDetail([FromQuery]UserDetailAdminSideQuery query,
                                                CancellationToken cancellationToken = default)
    {
        var res = await Mediator.Send(query);
        if (res == null) return JsonResponseStatus.Error(null, "کاربر موردنظر یافت نشده است.");

        return JsonResponseStatus.Success(res);
    }

    #endregion
}
