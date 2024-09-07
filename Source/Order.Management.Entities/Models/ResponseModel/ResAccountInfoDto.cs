namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class ResAccountInfoDto
    {
        public string UserName { get; set; } = null!;

        public string AccessToken { get; set; } = null!;
    }
}
