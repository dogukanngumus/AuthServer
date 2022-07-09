using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.Authentication;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using AuthServer.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthServer.Service.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserApp> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshToken;

    public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshToken)
    {
        _clients = clients.Value;
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _userRefreshToken = userRefreshToken;
    }

    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null)  throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Response<TokenDto>.Fail("Email or password is wrong", 400, false);

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Response<TokenDto>.Fail("Email or password is wronge",400, false);
        }

        var token = _tokenService.CreateToken(user);
        var userRefreshToken = await _userRefreshToken.Where(x => x.UserId == user.Id).
            SingleOrDefaultAsync();
        if (userRefreshToken == null)
        {
            await _userRefreshToken.AddAsync(new UserRefreshToken()
            {
                UserId = user.Id,
                Token = token.RefreshToken,
                Expiration = token.RefreshTokenExpiration
            });
        }else
        {
            userRefreshToken.Token = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();
        return Response<TokenDto>.Success(token, 200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshToken.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken == null)
        {
            return Response<TokenDto>.Fail("Refresh token not found",400,true);
        }

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
        if (user == null)
        {
            return Response<TokenDto>.Fail("User id not found", 400, true);
        }

        var tokenDto = _tokenService.CreateToken(user);
        existRefreshToken.Token = tokenDto.RefreshToken;
        existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
        await _unitOfWork.CommitAsync();
        
        return Response<TokenDto>.Success(tokenDto, 200);
    }

    public async Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshToken.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken == null)
        {
            return Response<NoContentDto>.Fail("Refresh token not found", 400 , true);
        }
        _userRefreshToken.Remove(existRefreshToken);
        await _unitOfWork.CommitAsync();
        return Response<NoContentDto>.Success(200);
    }

    public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients.SingleOrDefault(x=> x.Id == clientLoginDto.ClientId && x.Secret ==clientLoginDto.ClientSecret);
        if (client == null)
        {
            return Response<ClientTokenDto>.Fail("Client id or client secret not found", 400, true);
        }

        var token = _tokenService.CreateTokenByClient(client);
        return Response<ClientTokenDto>.Success(token, 200);
    }
}