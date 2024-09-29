namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqListOrderDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string? DateFrom { get; set; }

        public string? DateTo { get; set; }

        public IEnumerable<string>? Status { get; set; }
    }
}
