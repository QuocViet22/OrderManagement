using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;

namespace OrderManagement.Services.Service
{
    public class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork<OrderManagementDbContext> _unitOfWork;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AccountService> _logger;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialize a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public EmployeeService(IUnitOfWork<OrderManagementDbContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list employee
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ResEmployeeInfoDto>> GetListEmployee(TokenInfoModel tokenInfo)
        {
            try
            {
                var accountRepo = _unitOfWork.GetRepository<Account>();
                var data = new List<ResEmployeeInfoDto>();
                if (tokenInfo.RoleName == RoleName.admin.ToString())
                {
                    //Return all records for Admin role
                    var existedData = (await accountRepo.GetPagedListAsync(
                             pageIndex: 0,
                             pageSize: accountRepo.Count(),
                             include: i => i.Include(o => o.Employee))).Items;
                    data = _mapper.Map<IEnumerable<ResEmployeeInfoDto>>(existedData.Select(x => x.Employee)).ToList();
                }
                else if (tokenInfo.RoleName == RoleName.employee.ToString())
                {
                    //Return all records for Employee role
                    var existedData = (await accountRepo.GetPagedListAsync(
                             pageIndex: 0,
                             pageSize: accountRepo.Count(predicate: x => x.Role.Name == tokenInfo.RoleName && x.Employee.Name == tokenInfo.EmployeeName),
                             predicate: x => x.Role.Name == tokenInfo.RoleName && x.Employee.Name == tokenInfo.EmployeeName,
                             include: i => i.Include(o => o.Employee))).Items;
                    data = _mapper.Map<IEnumerable<ResEmployeeInfoDto>>(existedData.Select(x => x.Employee)).ToList();
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetListEmployee()");
                throw;
            }
        }
    }
}
