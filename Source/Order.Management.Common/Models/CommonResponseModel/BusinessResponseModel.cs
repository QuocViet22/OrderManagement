namespace OrderManagement.Common.Models.CommonResponseModel
{
    /// <summary>
    /// Define an business response model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusinessResponseModel<T>
    {
        public int StatusCode { get; set; }
        public T? Result { get; set; }
    }
}
