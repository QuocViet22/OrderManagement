namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqAccountInfoDto
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
