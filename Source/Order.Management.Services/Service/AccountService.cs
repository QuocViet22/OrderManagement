using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Helper;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;

namespace OrderManagement.Services.Service
{
    public class AccountService : IAccountService
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
        /// Initialize a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public AccountService(IUnitOfWork<OrderManagementDbContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get authentication for account
        /// </summary>
        /// <param name="accountInfoDto"></param>
        /// <returns></returns>
        public async Task<ResAccountInfoDto> GetAuthentication(ReqAccountInfoDto accountInfoDto)
        {
            try
            {
                var result = new ResAccountInfoDto();
                var accountRepo = _unitOfWork.GetRepository<Account>();
                var existedAccount = await accountRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.UserName == accountInfoDto.UserName && x.Password == accountInfoDto.Password,
                        include: i => i
                            .Include(o => o.Role)
                            .Include(o => o.Employee)
                    );
                var employeeInfoDto = _mapper.Map<ResEmployeeInfoDto>(existedAccount.Employee);
                if (existedAccount != null)
                {
                    var accessToken = JwtHandler.GetJwtToken(existedAccount);
                    result = new ResAccountInfoDto(existedAccount.UserName, accessToken, employeeInfoDto);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetAllAccounts()");
                throw;
            }
        }
    }
}
