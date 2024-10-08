﻿namespace OrderManagement.Entities.Entities;

public partial class Employee
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
