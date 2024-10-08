﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Services.Interface;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Common.Helper;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Models.ResponseModel;

namespace Orer.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<OrderController> _logger;

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost("AddNewOrder")]
        [Authorize]
        public async Task<IActionResult> AddNewOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var result = await _orderService.AddNewOrder(reqOrderInfoDto);
                var response = new ApiResponseModel<string>(result.Result, null);
                return StatusCode(result.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateOrder")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var result = await _orderService.UpdateOrder(reqOrderInfoDto);
                var response = new ApiResponseModel<string>(result.Result, null);
                return StatusCode(result.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetListOrder")]
        [Authorize]
        public async Task<IActionResult> GetListOrder(ReqListOrderDto reqListOrderDto)
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

                var result = await _orderService.GetListOrder(reqListOrderDto, tokenInfo);
                var response = new ApiResponseModel<IEnumerable<ResOrderInfoDto>>(ResponseMessage.SuccessfulMsg, result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderDetail")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetail(string orderId)
        {
            try
            {
                var orderGuid = new Guid(orderId);
                var result = await _orderService.GetOrderDetail(orderGuid);
                if (result == null)
                {
                    var notFoundResponse = new ApiResponseModel<string>(ResponseMessage.FailedToGetOrderMsg, null);
                    return StatusCode(500, notFoundResponse);
                }
                var response = new ApiResponseModel<ResOrderInfoDto>(ResponseMessage.SuccessfulMsg, result);
                return Ok(response);
            }
            catch (FormatException)
            {
                var response = new ApiResponseModel<string>(ResponseMessage.InvalidOrderIdMsg, null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
