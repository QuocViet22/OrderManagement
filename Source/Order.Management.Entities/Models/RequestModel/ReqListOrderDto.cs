namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqListOrderDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
