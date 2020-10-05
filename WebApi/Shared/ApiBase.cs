using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Shared
{
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public abstract class ApiBase : ControllerBase
    {
    }
}
