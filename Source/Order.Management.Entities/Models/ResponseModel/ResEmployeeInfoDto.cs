namespace OrderManagement.Entities.Models.ResponseModel
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class ResEmployeeInfoDto
    {
        public ResEmployeeInfoDto() { }

        public ResEmployeeInfoDto(Guid id, string name, string phoneNumber)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
        }
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
