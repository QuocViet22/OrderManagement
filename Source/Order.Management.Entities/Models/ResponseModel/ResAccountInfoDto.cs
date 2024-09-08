namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class ResAccountInfoDto
    {
        public ResAccountInfoDto() { }

        public ResAccountInfoDto(string userName, string accessToken, ResEmployeeInfoDto resEmployeeInfoDto)
        {
            UserName = userName;
            AccessToken = accessToken;
            EmployeeInfo = resEmployeeInfoDto;
        }

        public string UserName { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public ResEmployeeInfoDto EmployeeInfo { get; set; }
    }
}
