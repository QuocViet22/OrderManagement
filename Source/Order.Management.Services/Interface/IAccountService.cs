using OrerManagement.Api.Models;

namespace OrderManagement.Services.Interface
{
    /// <summary>
    /// The Account service interface
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        public Task<Account> GetAllAccounts();
    }
}
