using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.User;
using Microsoft.AspNetCore.Identity;

namespace BankListAPI.VsCode.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<string> GenerateToken(ApiUser apiUser);
        Task<string> CreateRefreshToken(ApiUser user);
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto req);
    }
}
