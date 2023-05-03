using BankListAPI.VsCode.Models.User;
using Microsoft.AspNetCore.Identity;

namespace BankListAPI.VsCode.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<bool> Login(LoginDto loginDto);
    }
}
