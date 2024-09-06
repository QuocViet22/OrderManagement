namespace OrderManagement.Entities.Models
{
    /// <summary>
    /// Define DTO model to return to client
    /// </summary>
    public class AccountInfoDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
