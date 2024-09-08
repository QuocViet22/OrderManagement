using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;

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
                    return Unauthorized(ResponseMessage.FailAuthorizeToken);
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleName = jwtToken.Claims.FirstOrDefault(c => c.Type == HelperConstants.RoleKey)?.Value;
                var employeeName = jwtToken.Claims.FirstOrDefault(c => c.Type == HelperConstants.EmployeeName)?.Value;

                if (roleName == null || employeeName == null)
                {
                    return BadRequest(ResponseMessage.FailAuthorizeToken);
                }

                var result = await _employeeService.GetListEmployee(roleName, employeeName);
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
