namespace OrderManagement.Common.Models.ResponseModel
{
    /// <summary>
    /// Define result model to return to client
    /// </summary>
    public class ApiResponseModel<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? ResultData { get; set; }
        public ApiResponseModel() { }
        public ApiResponseModel(string message, T resultData)
        {
            Message = message;
            ResultData = resultData;
        }
    }
}
