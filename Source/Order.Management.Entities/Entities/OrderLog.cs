namespace OrerManagement.Api.Models;

public partial class OrderLog
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public string CreateBy { get; set; } = null!;

    public DateOnly CreatedOn { get; set; }

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
