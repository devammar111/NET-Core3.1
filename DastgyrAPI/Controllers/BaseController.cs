using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace DastgyrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        
    }
}
