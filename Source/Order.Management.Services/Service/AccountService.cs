using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using OrerManagement.Api.Data;
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
                var claims = new[]
                {
                    new Claim("UserName", accountInfoDto.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, "user_id")
                };

                var keyBytes = Encoding.UTF8.GetBytes(HelperConstants.Secret);

                var key = new SymmetricSecurityKey(keyBytes);

                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    HelperConstants.Audience,
                    HelperConstants.Issuer,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new ResAccountInfoDto 
                { 
                    UserName = accountInfoDto.UserName,
                    AccessToken = tokenString
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function GetAllAccounts()");
                throw;
            }
        }
    }
}
