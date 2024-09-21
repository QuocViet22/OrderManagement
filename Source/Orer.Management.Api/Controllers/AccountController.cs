using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using OrderManagement.Common.Helper;

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
                        Message = ResponseMessage.FailedLoginMsg,
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

        [HttpPost("AddNewAccount")]
        [Authorize]
        public async Task<IActionResult> AddNewAccount(ReqAccountCreationDto reqAccountCreationDto)
        {
            try
            {
                // Extract the JWT token from the request header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return Unauthorized(ResponseMessage.FailedAuthorizeTokenMsg);

                var tokenInfo = JwtHandler.GetInfoFromToken(token);

                if (tokenInfo.RoleName == null || tokenInfo.EmployeeName == null)
                    return BadRequest(ResponseMessage.FailedAuthorizeTokenMsg);
                var result = await _accountService.AddNewAccount(tokenInfo, reqAccountCreationDto);
                var response = new ApiResponseModel<string>(result.Result, null);
                return StatusCode(result.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
