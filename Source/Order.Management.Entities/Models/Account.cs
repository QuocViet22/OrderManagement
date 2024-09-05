using System;
using System.Collections.Generic;

namespace Orer.Management.Api.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
