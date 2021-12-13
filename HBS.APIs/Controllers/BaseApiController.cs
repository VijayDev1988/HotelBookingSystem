using Microsoft.AspNetCore.Mvc;

namespace HBS.APIs.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
