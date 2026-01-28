using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("health")]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Ok" });

        [HttpGet]
        [Route("error")]
        public IActionResult Error() => throw new InvalidOperationException("Test error");
    }
}
