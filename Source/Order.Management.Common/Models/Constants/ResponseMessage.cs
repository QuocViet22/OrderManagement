namespace OrderManagement.Common.Models.Constants
{
    /// <summary>
    /// Reponse message constant
    /// </summary>
    public class ResponseMessage
    {
        protected ResponseMessage() { }
        public const string SuccessfulLoginMsg = "Đăng nhập thành công!";
        public const string FailedLoginMsg = "Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin!";
        public const string SuccessfulMsg = "Thành công!";
        public const string FailedGetDataMsg = "Không tìm thấy thông tin!";
        public const string FailedAuthorizeToken = "Thông tin access token không hợp lệ!";
    }
}
