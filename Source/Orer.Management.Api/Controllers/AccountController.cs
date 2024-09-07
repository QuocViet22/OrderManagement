﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Entities.Models.RequestModel;
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
                return Ok(await _accountService.GetAuthentication(accountInfoDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
