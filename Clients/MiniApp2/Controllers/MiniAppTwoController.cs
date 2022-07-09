using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp2.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MiniAppTwoController:ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var userName = HttpContext.User.Identity.Name;
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        return Ok($"User Name : {userName} - UserId : {userId}");
    }
}