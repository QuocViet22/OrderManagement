using OrderManagement.Entities.Models.RequestModel;

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
        public Task<string> AddNewOrder(ReqOrderInfoDto reqOrderInfoDto);

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="reqOrderInfoDto"></param>
        /// <returns></returns>
        public Task<string> UpdateOrder(ReqOrderInfoDto reqOrderInfoDto);
    }
}
