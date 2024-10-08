﻿namespace OrderManagement.Common.Models.Constants
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
        public const string FailedMsg = "Thất bại!";
        public const string FailedGetDataMsg = "Không tìm thấy thông tin!";
        public const string FailedAuthorizeTokenMsg = "Thông tin access token không hợp lệ!";
        public const string FailedToGetOrderMsg = "Không tìm thấy đơn!";
        public const string InvalidOrderIdMsg = "ID của đơn không hợp lệ!";
    }
}
