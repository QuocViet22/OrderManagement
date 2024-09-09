namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ResOrderLogInfoDto
    {
        public string Content { get; set; } = null!;

        public string CreateBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
