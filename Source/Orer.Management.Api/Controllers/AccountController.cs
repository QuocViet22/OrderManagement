using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;

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
                return Ok("Ok");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
