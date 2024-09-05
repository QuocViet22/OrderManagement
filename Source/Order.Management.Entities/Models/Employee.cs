using System;
using System.Collections.Generic;

namespace Orer.Management.Api.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
