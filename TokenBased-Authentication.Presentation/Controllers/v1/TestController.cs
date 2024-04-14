using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TokenBased_Authentication.Presentation.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public virtual IActionResult Test()
        {
            return Ok("Hello");
        }
    }
}
