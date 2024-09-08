namespace OrderManagement.Entities.Models.RequestModel
{
    /// <summary>
    /// Define DTO model to request from client
    /// </summary>
    public class ReqOrderInfoDto
    {
        public string? CustomerName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? JobTitle { get; set; }

        public string? JobDescription { get; set; }

        public string? Status { get; set; } = null!;

        public string? Signature { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? OrderId { get; set; }

        public string? Action {  get; set; }
    }
}
