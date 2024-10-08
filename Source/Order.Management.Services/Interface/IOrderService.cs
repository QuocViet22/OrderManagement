﻿using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;

namespace OrderManagement.Services.Interface
{
    /// <summary>
    /// The Order service
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="reqOrderInfoDto"></param>
        /// <returns></returns>
        public Task<BusinessResponseModel<string>> AddNewOrder(ReqOrderInfoDto reqOrderInfoDto);

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="reqOrderInfoDto"></param>
        /// <returns></returns>
        public Task<BusinessResponseModel<string>> UpdateOrder(ReqOrderInfoDto reqOrderInfoDto);

        /// <summary>
        /// Get list order
        /// </summary>
        /// <param name="reqListOrderDto"></param>
        /// <returns></returns>
        public Task<IEnumerable<ResOrderInfoDto>> GetListOrder(ReqListOrderDto reqListOrderDto, TokenInfoModel tokenInfo);

        /// <summary>
        /// Get order detail by ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Task<ResOrderInfoDto> GetOrderDetail(Guid orderId);
    }
}
