using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
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
                        predicate: x => x.Id == reqOrderInfoDto.OrderId,
                        include: i => i.Include(o => o.Employee)
                    );

                if (existedOrderData == null)
                    return new BusinessResponseModel<string>()
                    {
                        StatusCode = 400,
                        Result = ResponseMessage.FailedGetDataMsg
                    };

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
                if (tokenInfo.RoleName == RoleName.admin.ToString())
                {
                    //Return all records for Admin role
                    var data = (await orderRepo.GetPagedListAsync(
                            pageIndex: reqListOrderDto.PageIndex,
                            pageSize: reqListOrderDto.PageSize,
                            include: i => i
                                        .Include(o => o.Employee)
                                        .Include(o => o.OrderLogs)
                        )).Items;
                    result = _mapper.Map<List<ResOrderInfoDto>>(data);
                }
                else if (tokenInfo.RoleName == RoleName.employee.ToString())
                {
                    //Return all records for Employee role
                    var data = (await orderRepo.GetPagedListAsync(
                            pageIndex: reqListOrderDto.PageIndex,
                            pageSize: reqListOrderDto.PageSize,
                            predicate: x => x.Employee.Name == tokenInfo.EmployeeName,
                            include: i => i
                                        .Include(o => o.Employee)
                                        .Include(o => o.OrderLogs)
                        )).Items;
                    result = _mapper.Map<List<ResOrderInfoDto>>(data);
                }
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
    }
}
