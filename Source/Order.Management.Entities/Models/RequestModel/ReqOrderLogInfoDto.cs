namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqOrderLogInfoDto
    {
        public string Content { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
