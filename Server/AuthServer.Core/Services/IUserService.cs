using AuthServer.Core.Dtos;
using AuthServer.Shared.Dtos;

namespace AuthServer.Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
}