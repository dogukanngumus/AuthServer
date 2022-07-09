using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.Authentication;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers;

public class AuthController:BaseController
{
    private readonly IAuthenticationService _authenticationService;
    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);
        return CreateActionResult(result);
    }

    [HttpPost]
    public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result = _authenticationService.CreateTokenByClient(clientLoginDto);
        return CreateActionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);
        return CreateActionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
        return CreateActionResult(result);
    }
}