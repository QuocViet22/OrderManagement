namespace OrerManagement.Api.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
