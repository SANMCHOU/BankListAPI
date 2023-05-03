using AutoMapper;
using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankListAPI.VsCode.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        private const string _loginProvider = "BankListApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
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

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            bool isValidUser = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if(!isValidUser)
                {
                    return null;
                }
                var token = await GenerateToken(user);
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    RefreshToken = await CreateRefreshToken(user),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }.Union(roleClaims).Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings: Issuer"],
                audience: _configuration["JwtSettings: Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
        {
            var JwtSecurityTokenHandler =  new JwtSecurityTokenHandler();
            var tokenContent = JwtSecurityTokenHandler.ReadJwtToken(authResponseDto.Token);
            var userName = tokenContent.Claims.ToList().FirstOrDefault(a=>
                                                        a.Type == JwtRegisteredClaimNames.Email)?.Value;

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user, _loginProvider, _refreshToken,
                authResponseDto.Token);
            if (isValidRefreshToken)
            {
                var token = await GenerateToken(user);
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    RefreshToken= await CreateRefreshToken(user)
                };
            }

            await _userManager.UpdateSecurityStampAsync(user);
            return null;
        }

        public async Task<string> CreateRefreshToken(ApiUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, _loginProvider, _refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _loginProvider, _refreshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(user, _loginProvider, _refreshToken, 
                newRefreshToken);

            return newRefreshToken.ToString();
        }
    }
}
