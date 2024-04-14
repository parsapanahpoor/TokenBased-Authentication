using Microsoft.AspNetCore.Mvc;

namespace TokenBased_Authentication.Presentation.Controllers.v2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : Controllers.v1.TestController
    {
        public override IActionResult Test()
        {
            return Ok("hello2");
        }
    }
}
