namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqOrderLogInfoDto
    {
        public string Content { get; set; } = null!;

        public string CreateBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
