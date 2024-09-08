using Microsoft.IdentityModel.Tokens;
using OrderManagement.Common.Models.CommonResponseModel;
using OrderManagement.Common.Models.Constants;
using OrderManagement.Common.Setting;
using OrderManagement.Entities.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagement.Common.Helper
{
    public static class JwtHandler
    {
        /// <summary>
        /// Build base start up for services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static TokenInfoModel GetInfoFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleName = jwtToken.Claims.FirstOrDefault(c => c.Type == HelperConstants.RoleKey)?.Value;
            var employeeName = jwtToken.Claims.FirstOrDefault(c => c.Type == HelperConstants.EmployeeName)?.Value;
            return new TokenInfoModel() { RoleName = roleName, EmployeeName = employeeName };
        }

        /// <summary>
        /// Generate JWT Token
        /// </summary>
        /// <param name="existedAccount"></param>
        /// <returns></returns>
        public static string GetJwtToken(Account existedAccount)
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
    }
}
