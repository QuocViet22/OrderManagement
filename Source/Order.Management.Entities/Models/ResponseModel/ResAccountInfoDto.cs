namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class ResAccountInfoDto
    {
        public ResAccountInfoDto() { }

        public ResAccountInfoDto(string userName, string accessToken)
        {
            UserName = userName;
            AccessToken = accessToken;
        }

        public string UserName { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
