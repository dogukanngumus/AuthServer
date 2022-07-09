using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.Authentication;
using AuthServer.Shared.Dtos;

namespace AuthServer.Core.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    Task<Response<NoContentDto>> RevokeRefreshToken(string refreshToken);
    Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
}