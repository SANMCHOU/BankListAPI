using BankListAPI.VsCode.Core.Contracts;
using BankListAPI.VsCode.Core.Contracts;
using BankListAPI.VsCode.Core.Models.User;
using BankListAPI.VsCode.Core.Models.User;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger) { 
            this._authManager = authManager;
            this._logger = logger;
        }

        //POST: api/controller/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration attempt for {apiUserDto.Email}");
            try
            {
                if (apiUserDto == null)
                {
                    return BadRequest();
                }
                var errors = await _authManager.Register(apiUserDto);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Something went wrong in the {nameof(Register)} - user registration attempt for {apiUserDto.Email}");

                return Problem($"Something went wrong in the {nameof(Register)} - Please contact support", statusCode: 500);
            }
           
        }

        //POST: api/controller/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"Login attempt for {loginDto.Email}");
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Something went wrong in the {nameof(Login)} - user login attempt for {loginDto.Email}");
                return Problem($"Something went wrong in the {nameof(Login)} - Please contact support", statusCode: 500);
            }
           
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
