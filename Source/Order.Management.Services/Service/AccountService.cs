using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.Extensions.Logging;
using OrderManagement.Services.Interface;
using OrerManagement.Api.Data;
using OrerManagement.Api.Models;

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
        /// Initialize a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        public AccountService(IUnitOfWork<OrderManagementDbContext> unitOfWork, ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Get all accounts from DB
        /// </summary>
        /// <returns></returns>
        public async Task<Account> GetAllAccounts()
        {
            try
            {
                var accountRepo = _unitOfWork.GetRepository<Account>();
                var data = await accountRepo.GetFirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetAllAccounts()");
                throw;
            }
        }
    }
}
