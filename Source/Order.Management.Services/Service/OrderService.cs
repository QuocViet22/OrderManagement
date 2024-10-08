﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Helper;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using System.Linq.Expressions;

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
        public async Task<BusinessResponseModel<string>> AddNewOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var employeeData = await employeeRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.EmployeeId
                    );

                if (employeeData == null)
                    return new BusinessResponseModel<string>()
                    {
                        StatusCode = 400,
                        Result = ResponseMessage.FailedGetDataMsg
                    };
                    
                var currentEmployeeName = employeeData.Name;
                var currentTime = DateTime.Now;
                var firstOrderLogData = new ReqOrderLogInfoDto()
                {
                    Content = $"{currentEmployeeName} {HelperConstants.AddNewOrderLogMsg}",
                    CreatedBy = $"{currentEmployeeName}",
                    CreatedOn = currentTime,
                };

                reqOrderInfoDto.Status = OrderStatus.New.ToString();
                var orderLogData = _mapper.Map<OrderLog>(firstOrderLogData);
                var orderData = _mapper.Map<Order>(reqOrderInfoDto);
                orderData.OrderLogs.Add(orderLogData);
                orderData.CreatedBy = currentEmployeeName;
                orderData.CreatedOn = currentTime;
                orderData.ModifiedBy = currentEmployeeName;
                orderData.ModifiedOn = currentTime;
                orderRepo.Insert(orderData);
                _unitOfWork.SaveChanges();
                return new BusinessResponseModel<string>()
                {
                    StatusCode = 200,
                    Result = ResponseMessage.SuccessfulMsg
                };
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
        public async Task<BusinessResponseModel<string>> UpdateOrder(ReqOrderInfoDto reqOrderInfoDto)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var existedOrderData = await orderRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.OrderId
                    );

                var existedEmployeeData = await employeeRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == reqOrderInfoDto.EmployeeId
                    );

                if (existedOrderData == null || existedEmployeeData == null)
                    return new BusinessResponseModel<string>()
                    {
                        StatusCode = 400,
                        Result = ResponseMessage.FailedGetDataMsg
                    };

                var currentTime = DateTime.Now;
                var currentEmployeeName = existedEmployeeData.Name;

                switch (reqOrderInfoDto.Action)
                {
                    case HelperConstants.ActiveAction:
                        var activeOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{currentEmployeeName} {HelperConstants.ActivedOrderLogMsg}",
                            CreatedBy = $"{currentEmployeeName}",
                            CreatedOn = currentTime,
                        };
                        var activeOrderLogData = _mapper.Map<OrderLog>(activeOrderLogDto);
                        existedOrderData.OrderLogs.Add(activeOrderLogData);
                        existedOrderData.ModifiedBy = currentEmployeeName;
                        existedOrderData.ModifiedOn = currentTime;
                        existedOrderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.UpdateAction:
                        var updatedOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{currentEmployeeName} {HelperConstants.UpdatedOrderLogMsg}",
                            CreatedBy = $"{currentEmployeeName}",
                            CreatedOn = currentTime,
                        };
                        var updateOrderLogData = _mapper.Map<OrderLog>(updatedOrderLogDto);
                        existedOrderData.OrderLogs.Add(updateOrderLogData);
                        existedOrderData.CustomerName = reqOrderInfoDto.CustomerName;
                        existedOrderData.PhoneNumber = reqOrderInfoDto.PhoneNumber;
                        existedOrderData.Address = reqOrderInfoDto.Address;
                        existedOrderData.JobTitle = reqOrderInfoDto.JobTitle;
                        existedOrderData.JobDescription = reqOrderInfoDto.JobDescription;
                        existedOrderData.ModifiedBy = currentEmployeeName;
                        existedOrderData.ModifiedOn = currentTime;
                        existedOrderData.Status = OrderStatus.Active.ToString();
                        break;
                    case HelperConstants.DoneAction:
                        var completedOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{currentEmployeeName} {HelperConstants.CompletedOrderLogMsg}",
                            CreatedBy = $"{currentEmployeeName}",
                            CreatedOn = currentTime,
                        };
                        var completedOrderLogData = _mapper.Map<OrderLog>(completedOrderLogDto);
                        existedOrderData.Signature = reqOrderInfoDto.Signature;
                        existedOrderData.OrderLogs.Add(completedOrderLogData);
                        existedOrderData.ModifiedBy = currentEmployeeName;
                        existedOrderData.ModifiedOn = currentTime;
                        existedOrderData.Status = OrderStatus.Done.ToString();
                        break;
                    case HelperConstants.CancelledAction:
                        var canceledOrderLogDto = new ReqOrderLogInfoDto()
                        {
                            Content = $"{currentEmployeeName} {HelperConstants.CanceledOrderLogMsg}",
                            CreatedBy = $"{currentEmployeeName}",
                            CreatedOn = currentTime,
                        };
                        var canceledOrderLogData = _mapper.Map<OrderLog>(canceledOrderLogDto);
                        existedOrderData.OrderLogs.Add(canceledOrderLogData);
                        existedOrderData.ModifiedBy = currentEmployeeName;
                        existedOrderData.ModifiedOn = currentTime;
                        existedOrderData.Status = OrderStatus.Cancelled.ToString();
                        break;
                }
                orderRepo.Update(existedOrderData);
                _unitOfWork.SaveChanges();
                return new BusinessResponseModel<string>()
                {
                    StatusCode = 200,
                    Result = ResponseMessage.SuccessfulMsg
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function UpdateOrder()");
                throw;
            }
        }

        /// <summary>
        /// Get order list
        /// </summary>
        /// <param name="reqListOrderDto"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ResOrderInfoDto>> GetListOrder(ReqListOrderDto reqListOrderDto, TokenInfoModel tokenInfo)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var result = new List<ResOrderInfoDto>();
                var orderPredicate = GetOrderPredicate(reqListOrderDto, tokenInfo);
                var data = (await orderRepo.GetPagedListAsync(
                        pageIndex: reqListOrderDto.PageIndex,
                        pageSize: reqListOrderDto.PageSize,
                        predicate: orderPredicate,
                        include: i => i
                                    .Include(o => o.Employee)
                                    .Include(o => o.OrderLogs),
                        orderBy: x => x.OrderByDescending(o => o.CreatedOn)
                    )).Items;
                result = _mapper.Map<List<ResOrderInfoDto>>(data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetListOrder()");
                throw;
            }
        }

        /// <summary>
        /// Get order detail by ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResOrderInfoDto> GetOrderDetail(Guid orderId)
        {
            try
            {
                var orderRepo = _unitOfWork.GetRepository<Order>();
                var result = new ResOrderInfoDto();
                //Return record of Admin role
                var data = await orderRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.Id == orderId,
                        include: i => i
                                    .Include(o => o.Employee)
                                    .Include(o => o.OrderLogs)
                    );
                result = _mapper.Map<ResOrderInfoDto>(data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetOrderDetail()");
                throw;
            }
        }

        /// private service
        #region
        /// <summary>
        /// private predicate
        /// </summary>
        /// <param name="reqListOrderDto"></param>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        private static Expression<Func<Order, bool>> GetOrderPredicate(ReqListOrderDto reqListOrderDto, TokenInfoModel tokenInfo)
        {
            var dateFromValue = StringUltility.ConvertStringToDateTime(reqListOrderDto.DateFrom);
            var dateToValue = StringUltility.ConvertStringToDateTime(reqListOrderDto.DateTo);
            return x =>
                ( // By Time
                 (!dateFromValue.HasValue && !dateToValue.HasValue)
                 || (dateFromValue.HasValue && !dateToValue.HasValue && x.CreatedOn >= dateFromValue)
                 || (!dateFromValue.HasValue && dateToValue.HasValue && x.CreatedOn <= dateToValue)
                 || (dateFromValue.HasValue && dateToValue.HasValue && x.CreatedOn >= dateFromValue && x.CreatedOn <= dateToValue)
                )
                && ( // By Status
                  reqListOrderDto.Status == null
                  || !reqListOrderDto.Status.Any()
                  || reqListOrderDto.Status.Contains(x.Status)
                )
                && ( // Is Employee role
                  tokenInfo.RoleName == RoleName.admin.ToString()
                  || tokenInfo.RoleName == RoleName.employee.ToString() && x.Employee.Name == tokenInfo.EmployeeName
                );
        }
        #endregion
    }
}
