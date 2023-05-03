using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankListAPI.VsCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager) { 
            this._authManager = authManager;
        }

        //POST: api/controller/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            if (apiUserDto == null)
            {
                return BadRequest();
            }
            var errors = await _authManager.Register(apiUserDto);
            if(errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        //POST: api/controller/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest();
            }
            var isValidUserResponse = await _authManager.Login(loginDto);
            if (isValidUserResponse.Token == null)
            {
                return Unauthorized(); //401
            }
            return Ok(isValidUserResponse);
        }

        //POST: api/controller/refreshToken
        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDto req)
        {
            if (req == null)
            {
                return BadRequest();
            }
            var isValidUserResponse = await _authManager.VerifyRefreshToken(req);
            if (isValidUserResponse.Token == null)
            {
                return Unauthorized(); //401
            }
            return Ok(isValidUserResponse);
        }
    }
}
