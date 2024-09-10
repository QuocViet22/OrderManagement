using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;

namespace OrderManagement.Services.Interface
{
    /// <summary>
    /// The Account service interface
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Get authentication for account
        /// </summary>
        /// <param name="accountInfoDto"></param>
        /// <returns></returns>
        public Task<ResAccountInfoDto> GetAuthentication(ReqAccountInfoDto accountInfoDto);

        /// <summary>
        /// Add new account
        /// </summary>
        /// <param name="reqAccountCreationDto"></param>
        /// <returns></returns>
        public Task<string> AddNewAccount(TokenInfoModel tokenInfoModel, ReqAccountCreationDto reqAccountCreationDto);
    }
}
