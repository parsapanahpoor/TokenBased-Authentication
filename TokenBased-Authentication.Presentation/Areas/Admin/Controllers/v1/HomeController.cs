using EquipmentManagement.Presentation.HttpManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Domain.IRepositories.Role;
using TokenBased_Authentication.Presentation.Areas.Admin.ActionFilterAttributes;
namespace TokenBased_Authentication.Presentation.Areas.Admin.Controllers.v1;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/Admin/[controller]")]

public class HomeController : AdminBaseController
{
    #region Admin Dashboard

    [HttpGet("AdminDashboard")]
	public async Task<IActionResult> AdminDashboard(CancellationToken cancellationToken)
	{
		return Ok(JsonResponseStatus.Success(null , "Wellcome to admin dashboard"));
	}

	#endregion
}
