using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace Orer.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(ReqAccountInfoDto accountInfoDto)
        {
            try
            {
                _logger.LogInformation("UserName ===> {0}", accountInfoDto.UserName);
                var data = await _accountService.GetAuthentication(accountInfoDto);

                if (data.UserName == null && data.AccessToken == null)
                {
                    return StatusCode(500, new ApiResponseModel<string>()
                    {
                        Message = ResponseMessage.FailLoginMsg,
                    });
                }

                var response = new ApiResponseModel<ResAccountInfoDto>(ResponseMessage.SuccessfulLoginMsg, data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "$login");
                return BadRequest(new ApiResponseModel<string>()
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("Test")]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            try
            {
                // Extract the JWT token from the request header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("JWT token is missing.");
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "RoleKey")?.Value;

                if (roleClaim == "admin")
                {
                    // Return all records for Admin role
                    return Ok("Admin");
                }
                else if (roleClaim == "employee")
                {
                    // Return filtered records for Employee role
                    return Ok("Employee");
                }

                return Forbid("Unauthorized access.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
