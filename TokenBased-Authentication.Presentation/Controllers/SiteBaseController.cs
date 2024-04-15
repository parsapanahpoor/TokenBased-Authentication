using MediatR;
using Microsoft.AspNetCore.Mvc;
using TokenBased_Authentication.Presentation.Filter;

namespace TokenBased_Authentication.Presentation.Controllers.v1;

[ApiController]
[CatchExceptionFilter]

public class SiteBaseController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
