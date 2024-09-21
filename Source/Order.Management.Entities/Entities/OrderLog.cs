namespace OrderManagement.Entities.Entities;

public partial class OrderLog
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
