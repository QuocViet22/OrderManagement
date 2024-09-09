using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                    Content = $"{employeeData.Name} {HelperConstants.AddNewOrderLogMsg}",
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

                var existedOrderData = await orderRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.OrderId,
                        include: i => i.Include(o => o.Employee)
                    );

                if (existedOrderData == null)
                    return ResponseMessage.FailedGetDataMsg;

                switch (reqOrderInfoDto.Action)
                {
                    case HelperConstants.ActiveAction:
                        var activeOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{existedOrderData.Employee.Name} {HelperConstants.ActivedOrderLogMsg}",
                            CreateBy = $"{existedOrderData.Employee.Name}",
                            CreatedOn = DateTime.Now,
                        };
                        var activeOrderLogData = _mapper.Map<OrderLog>(activeOrderLogDto);
                        existedOrderData.OrderLogs.Add(activeOrderLogData);
                        existedOrderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.UpdateAction:
                        var updatedOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{existedOrderData.Employee.Name} {HelperConstants.UpdatedOrderLogMsg}",
                            CreateBy = $"{existedOrderData.Employee.Name}",
                            CreatedOn = DateTime.Now,
                        };
                        var updateOrderLogData = _mapper.Map<OrderLog>(updatedOrderLogDto);
                        existedOrderData.OrderLogs.Add(updateOrderLogData);
                        existedOrderData.CustomerName = reqOrderInfoDto.CustomerName;
                        existedOrderData.PhoneNumber = reqOrderInfoDto.PhoneNumber;
                        existedOrderData.Address = reqOrderInfoDto.Address;
                        existedOrderData.JobTitle = reqOrderInfoDto.JobTitle;
                        existedOrderData.JobDescription = reqOrderInfoDto.JobDescription;
                        existedOrderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.DoneAction:
                        var completedOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{existedOrderData.Employee.Name} {HelperConstants.CompletedOrderLogMsg}",
                            CreateBy = $"{existedOrderData.Employee.Name}",
                            CreatedOn = DateTime.Now,
                        };
                        var completedOrderLogData = _mapper.Map<OrderLog>(completedOrderLogDto);
                        existedOrderData.Signature = reqOrderInfoDto.Signature;
                        existedOrderData.OrderLogs.Add(completedOrderLogData);
                        existedOrderData.Status = OrderStatus.Done.ToString();
                        break;
                }
                orderRepo.Update(existedOrderData);
                _unitOfWork.SaveChanges();
                return ResponseMessage.SuccessfulMsg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function UpdateOrder()");
                throw;
            }
        }
    }
}
