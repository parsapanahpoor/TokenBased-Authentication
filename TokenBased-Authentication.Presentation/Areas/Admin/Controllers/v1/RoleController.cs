using ClinicManagement.Application.CQRS.AdminSide.Role.Command;
using EquipmentManagement.Presentation.HttpManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Command;
using TokenBased_Authentication.Application.CQRS.APIClient.v1.AdminSide.Role.Query;
using TokenBased_Authentication.Application.StaticTools;
using TokenBased_Authentication.Domain.DTO.AdminSide.Role;

namespace TokenBased_Authentication.Presentation.Areas.Admin.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Admin/[controller]")]
[Authorize]
public class RoleController : AdminBaseController
{
    #region Filter Roles

    [HttpGet("FilterRoles")]
    public async Task<IActionResult> FilterRoles(string? RoleTitle,
                                                 CancellationToken cancellation = default)
    {
        var res = await Mediator.Send(new FilterRolesQuery()
        {
            RoleTitle = RoleTitle
        }, cancellation);

        return Ok(JsonResponseStatus.Success(res));
    }

    #endregion

    #region Create Role 

    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO model,
                                                CancellationToken cancellationToken = default)
    {
        var res = await Mediator.Send(new CreateRoleCommand()
        {
            Permissions = model.Permissions,
            RoleUniqueName = model.RoleUniqueName,
            Title = model.Title
        }, cancellationToken);

        if (res)
        {
            return Ok(JsonResponseStatus.Success(res, "نقش جدید باموفقیت اضافه شده است."));
        }

        return Ok(JsonResponseStatus.Error(res, "اطلاعات وارد شده صحیح می باشد."));
    }

    #endregion

    #region Edit Role 

    [HttpPost("EditRole")]
    public async Task<IActionResult> EditRole([FromBody] EditRoleDTO model,
                                              CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new EditRoleCommand()
        {
            Id = model.Id,
            Permissions = model.Permissions,
            RoleUniqueName = model.RoleUniqueName,
            Title = model.Title
        }, cancellationToken);

        switch (res)
        {
            case EditRoleResult.Success:
                return Ok(JsonResponseStatus.Success(null, "عملیات با موفقیت انجام شد ."));

            case EditRoleResult.RoleNotFound:
                return Ok(JsonResponseStatus.Error(null, "نقش مورد نظر یافت نشد ."));

            case EditRoleResult.UniqueNameExists:
                return Ok(JsonResponseStatus.Error(null, "نام یکتا از قبل موجود است ."));
        }

        return Ok(JsonResponseStatus.Error(null, "نام یکتا از قبل موجود است ."));
    }

    #endregion

    #region Role Detail

    [HttpGet("RoleDetail")]
    public async Task<IActionResult> RoleDetail([FromQuery]ulong roleId,
                                                CancellationToken cancellation)
    {
        var model = await Mediator.Send(new EditRoleQuery() { RoleId = roleId }, cancellation);
        if (model == null) return Ok(JsonResponseStatus.Error(null, "نقش مورد نظر یافت نشده است."));

        return Ok(JsonResponseStatus.Success(model , "نقش مورد نظر باموفقیت یافت شده است."));
    }

    #endregion

    #region Delete Role 

    [HttpGet("DeleteRole")]
    public async Task<IActionResult> DeleteRole([FromQuery]ulong roleId,
                                                CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteRoleCommand() { RoleId = roleId }, cancellationToken);
        if (result) return Ok(JsonResponseStatus.Success(null, "نقش موردنظر باموفقیت انجام شده است."));

        return Ok(JsonResponseStatus.Error(null, "نقش موردنظر باموفقیت انجام شده است."));
    }

    #endregion
}
