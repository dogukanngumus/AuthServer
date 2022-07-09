using AuthServer.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController:ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(Response<T> response)
    {
        if (response.StatusCode == 204)
        {
            return new ObjectResult(null)
            {
                StatusCode = 204
            };
        }

        return new ObjectResult(response)
        {
            StatusCode = response.StatusCode
        };
    }
}