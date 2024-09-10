using OrderManagement.Entities.Entities;

namespace OrderManagement.Entities.Models.RequestModel
{
    public class ReqAccountCreationDto
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string RoleNameKey { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public string EmployeePhoneNumber { get; set; } = null!;

        public Employee? Employee { get; set; }
    }
}
