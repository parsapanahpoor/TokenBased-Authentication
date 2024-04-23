using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Presentation.Areas.Admin.ActionFilterAttributes;
using TokenBased_Authentication.Presentation.Filter;

namespace TokenBased_Authentication.Presentation.Areas.Admin.Controllers.v1;

[ApiController]
[CatchExceptionFilter]
[Authorize]
[CheckUserHasAnyRole]

public class AdminBaseController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}