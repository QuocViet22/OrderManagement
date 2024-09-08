using System.ComponentModel;

namespace OrderManagement.Common.Models.Constants
{
    public class HelperConstants
    {
        protected HelperConstants() { }

        /// <summary>
        /// JWT Token constants
        /// </summary>
        public const string UserName = "UserName";
        public const string Role = "Role";
        public const string RoleKey = "RoleKey";
        public const string UserId = "user_id";
        public const string EmployeeName = "EmployeeName";

        /// <summary>
        /// Order log message constants
        /// </summary>
        public const string CreateNewOrderLogMsg = "tạo đơn hàng thành công";

        /// <summary>
        /// Order action constants
        /// </summary>
        public const string ActiveAction = "Active";
        public const string UpdateAction = "CheckIn";
        public const string DoneAction = "Done";

    }

    /// <summary>
    /// Order status constants
    /// </summary>
    public enum OrderStatus
    {
        New,
        Active,
        Done
    }

    /// <summary>
    /// Role name constants
    /// </summary>
    public enum RoleName
    {
        admin,
        employee
    }
}
