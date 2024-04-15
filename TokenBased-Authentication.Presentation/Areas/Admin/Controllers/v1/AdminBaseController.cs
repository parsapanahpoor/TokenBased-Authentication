using MediatR;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Presentation.Filter;

namespace TokenBased_Authentication.Presentation.Areas.Admin.Controllers.v1;

[ApiController]
[CatchExceptionFilter]

public class AdminBaseController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}