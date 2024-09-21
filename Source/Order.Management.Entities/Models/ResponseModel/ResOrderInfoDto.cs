using OrderManagement.Entities.Entities;

namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class ResOrderInfoDto
    {
        public Guid Id { get; set; }

        public string? CustomerName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? JobTitle { get; set; }

        public string? JobDescription { get; set; }

        public string Status { get; set; } = null!;

        public string? Signature { get; set; }

        public Guid EmployeeId { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedOn { get; set; }

        public ResEmployeeInfoDto ResEmployeeInfoDto { get; set; } = null!;

        public List<ResOrderLogInfoDto> ResOrderLogInfoList { get; set; } = new List<ResOrderLogInfoDto>();
    }
}
