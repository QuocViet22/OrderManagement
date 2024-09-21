using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using OrderManagement.Common.Helper;

namespace Orer.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<EmployeeController> _logger;

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("GetListEmployee")]
        [Authorize]
        public async Task<IActionResult> GetListEmployee()
        {
            try
            {
                // Extract the JWT token from the request header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(ResponseMessage.FailedAuthorizeTokenMsg);
                }

                var tokenInfo = JwtHandler.GetInfoFromToken(token);

                if (tokenInfo.RoleName == null || tokenInfo.EmployeeName == null)
                {
                    return BadRequest(ResponseMessage.FailedAuthorizeTokenMsg);
                }

                var result = await _employeeService.GetListEmployee(tokenInfo);
                var response = new ApiResponseModel<IEnumerable<ResEmployeeInfoDto>>(ResponseMessage.SuccessfulMsg, result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
