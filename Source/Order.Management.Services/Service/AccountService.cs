using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.Common.Helper;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.RequestModel;
using OrderManagement.Entities.Models.ResponseModel;
using OrderManagement.Services.Interface;
using System.Security.Cryptography;
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
                var hashPassword = HashPassword(accountInfoDto.Password);
                var existedAccount = await accountRepo.GetFirstOrDefaultAsync(
                        predicate: x => x.UserName == accountInfoDto.UserName && x.Password == hashPassword,
                        include: i => i
                            .Include(o => o.Role)
                            .Include(o => o.Employee)
                    );
                if (existedAccount != null)
                {
                    var employeeInfoDto = _mapper.Map<ResEmployeeInfoDto>(existedAccount.Employee);
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

        /// <summary>
        /// Add new account
        /// </summary>
        /// <param name="reqAccountCreationDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BusinessResponseModel<string>> AddNewAccount(TokenInfoModel tokenInfo, ReqAccountCreationDto reqAccountCreationDto)
        {
            try
            {
                var result = new BusinessResponseModel<string>()
                {
                    StatusCode = 400,
                    Result = ResponseMessage.FailedMsg
                };
                var accountRepo = _unitOfWork.GetRepository<Account>();
                var roleRepo = _unitOfWork.GetRepository<Role>();
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                var data = new List<ResEmployeeInfoDto>();
                if (tokenInfo.RoleName == RoleName.admin.ToString())
                {
                    var checkExistedAccount = await accountRepo.GetFirstOrDefaultAsync(
                                                    predicate: x => x.UserName == reqAccountCreationDto.UserName
                                                );
                    var checkExistedEmployee = await employeeRepo.GetFirstOrDefaultAsync(
                                                    predicate: x => x.Name == reqAccountCreationDto.EmployeeName
                                                );
                    var checkExistedRole = await roleRepo.GetFirstOrDefaultAsync(
                                                    predicate: x => x.Key == reqAccountCreationDto.RoleNameKey
                                                );
                    if (checkExistedAccount == null && checkExistedEmployee == null && checkExistedRole != null)
                    {
                        var adminRole = await roleRepo.GetFirstOrDefaultAsync(
                                predicate: x => x.Key == reqAccountCreationDto.RoleNameKey,
                                selector: x => x.Id
                            );
                        reqAccountCreationDto.Password = HashPassword(reqAccountCreationDto.Password);
                        var newEmployee = new Employee()
                        {
                            Name = reqAccountCreationDto.EmployeeName,
                            PhoneNumber = reqAccountCreationDto.EmployeePhoneNumber,
                        };
                        reqAccountCreationDto.Employee = newEmployee;
                        var newAccountData = _mapper.Map<Account>(reqAccountCreationDto);
                        newAccountData.RoleId = adminRole;
                        accountRepo.Insert(newAccountData);
                        _unitOfWork.SaveChanges();
                        result = new BusinessResponseModel<string>()
                        {
                            StatusCode = 200,
                            Result = ResponseMessage.SuccessfulMsg
                        };
                    }
                    else
                    {
                        result = new BusinessResponseModel<string>()
                        {
                            StatusCode = 400,
                            Result = "Vui lòng kiểm tra lại thông tin người dùng!"
                        };
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while get data from function AddNewAccount()");
                throw;
            }
        }

        /// private services
        #region
        private static string HashPassword(string password)
        {
            // Convert the input password string to a byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Use SHA256 to hash the password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hashed bytes back to a string (hexadecimal)
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }
        #endregion
    }
}
