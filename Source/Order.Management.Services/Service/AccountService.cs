using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
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
                    var accessToken = GetJwtToken(existedAccount);
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

        #region
        private string GetJwtToken(Account existedAccount)
        {
            var claims = new[]
{
                    new Claim(HelperConstants.UserName, existedAccount.UserName),
                    new Claim(HelperConstants.Role, existedAccount.Role.Name),
                    new Claim(HelperConstants.RoleKey, existedAccount.Role.Key),
                    new Claim(HelperConstants.EmployeeName, existedAccount.Employee.Name),
                    new Claim(JwtRegisteredClaimNames.Sub, HelperConstants.UserId)
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
