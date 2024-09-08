using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Helper;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Services.Interface;

namespace OrderManagement.Services.Service
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork<OrderManagementDbContext> _unitOfWork;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<OrderService> _logger;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialize a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public OrderService(IUnitOfWork<OrderManagementDbContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="reqOrderInfoDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> AddNewOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var employeeData = await employeeRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.EmployeeId
                    );

                if (employeeData == null)
                    return ResponseMessage.FailedGetDataMsg;

                var firstOrderLogData = new ReqOrderLogInfoDto()
                {
                    Content = $"{employeeData.Name} {HelperConstants.CreateNewOrderLogMsg}",
                    CreateBy = $"{employeeData.Name}",
                    CreatedOn = DateTime.Now,
                };

                reqOrderInfoDto.Status = OrderStatus.New.ToString();
                var orderLogData = _mapper.Map<OrderLog>(firstOrderLogData);
                var orderData = _mapper.Map<Order>(reqOrderInfoDto);
                orderData.OrderLogs.Add(orderLogData);
                orderRepo.Insert(orderData);
                _unitOfWork.SaveChanges();
                return ResponseMessage.SuccessfulMsg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function AddNewOrder()");
                throw;
            }
        }

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="reqOrderInfoDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> UpdateOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var orderData = await orderRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.OrderId
                    );

                if (orderData == null)
                    return ResponseMessage.FailedGetDataMsg;

                switch (reqOrderInfoDto.Action)
                {
                    case HelperConstants.ActiveAction:
                        orderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.UpdateAction:
                        orderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.DoneAction:
                        orderData.Status = OrderStatus.Done.ToString();
                        break;
                }
                orderRepo.Update(orderData);
                _unitOfWork.SaveChanges();
                return ResponseMessage.SuccessfulMsg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function AddNewOrder()");
                throw;
            }
        }
    }
}
