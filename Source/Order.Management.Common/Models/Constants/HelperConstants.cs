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
        public const string AddNewOrderLogMsg = "tạo đơn hàng thành công";
        public const string ActivedOrderLogMsg = "đã chuyển trạng thái đơn hàng thành Active";
        public const string UpdatedOrderLogMsg = "đã cập nhật thông tin đơn hàng thành công";
        public const string CompletedOrderLogMsg = "đã hoàn thành đơn hàng";
        public const string CanceledOrderLogMsg = "đã hủy đơn hàng";

        /// <summary>
        /// Order action constants
        /// </summary>
        public const string ActiveAction = "Active";
        public const string UpdateAction = "Update";
        public const string DoneAction = "Done";
        public const string CancelledAction = "Cancelled";

    }

    /// <summary>
    /// Order status constants
    /// </summary>
    public enum OrderStatus
    {
        New,
        Active,
        Done,
        Canceled
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
