using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers;

public class UserController:BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        return CreateActionResult(await _userService.CreateUserAsync(createUserDto));
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        return CreateActionResult(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
    }
}