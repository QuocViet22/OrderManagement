using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        public async Task<IEnumerable<ResEmployeeInfoDto>> GetListEmployee(string roleName, string employeeName)
        {
            try
            {
                var accountRepo = _unitOfWork.GetRepository<Account>();
                var data = new List<ResEmployeeInfoDto>();
                if (roleName == HelperConstants.AdminRoleName)
                {
                    //Return all records for Admin role
                    var existedData = (await accountRepo.GetPagedListAsync(
                             pageIndex: 0,
                             pageSize: accountRepo.Count(),
                             include: i => i.Include(o => o.Employee))).Items;
                    data = _mapper.Map<IEnumerable<ResEmployeeInfoDto>>(existedData.Select(x => x.Employee)).ToList();
                }
                else if (roleName == HelperConstants.EmployeeRoleName)
                {
                    //Return all records for Employee role
                    var existedData = (await accountRepo.GetPagedListAsync(
                             pageIndex: 0,
                             pageSize: accountRepo.Count(predicate: x => x.Role.Name == roleName && x.Employee.Name == employeeName),
                             predicate: x => x.Role.Name == roleName && x.Employee.Name == employeeName,
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
