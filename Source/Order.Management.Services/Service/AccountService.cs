using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Setting;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                    );

                if (existedAccount != null)
                {
                    var accessToken = GetJwtToken(existedAccount);
                    result = new ResAccountInfoDto(existedAccount.UserName, accessToken);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetAllAccounts()");
                throw;
            }
        }

        #region
        private string GetJwtToken(Account existedAccount)
        {
            var claims = new[]
{
                    new Claim("UserName", existedAccount.UserName),
                    new Claim("Role", existedAccount.Role.Name),
                    new Claim("RoleKey", existedAccount.Role.Key),
                    new Claim(JwtRegisteredClaimNames.Sub, "user_id")
                };

            var keyBytes = Encoding.UTF8.GetBytes(ApplicationOptions.JwtConfig.Secret);

            var key = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                ApplicationOptions.JwtConfig.Audience,
                ApplicationOptions.JwtConfig.Issuer,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
        #endregion
    }
}
