namespace OrderManagement.Entities.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public string? CustomerName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? JobTitle { get; set; }

    public string? JobDescription { get; set; }

    public string Status { get; set; } = null!;

    public string? Signature { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public Guid EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();
}
