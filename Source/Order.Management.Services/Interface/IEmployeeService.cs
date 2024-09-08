using OrderManagement.Entities.Entities;
using OrderManagement.Entities.Models.ResponseModel;

namespace OrderManagement.Services.Interface
{
    /// <summary>
    /// The employee service interface
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Get list employee
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ResEmployeeInfoDto>> GetListEmployee(string roleName, string employeeName);
    }
}
