using AutoMapper;
using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.User;
using Microsoft.AspNetCore.Identity;

namespace BankListAPI.VsCode.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager) {
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
           var user = _mapper.Map<ApiUser>(userDto);
           user.UserName = userDto.Email;

           var res = await _userManager.CreateAsync(user, userDto.Password);
           if(res.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
            }

            return res.Errors;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            bool isValidUser = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isValidUser;
            
        }
    }
}
